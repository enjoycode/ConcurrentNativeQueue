**[English](README.en-US.md)** | 简体中文

# ConcurrentNativeQueue\<T\>

高性能无锁 MPSC（多生产者单消费者）原生队列，基于分段链表设计，适用于 .NET 6+。

槽位数据使用 `NativeMemory` 分配，**零 GC 压力**；段满时自动分配新段链接到尾部，段消费完后自动释放，无需手动管理容量。

> 注意：这是一个完全使用 `Claude Opus 4.6 High Thinking` 编写的项目。

> 虽然通过单元测试，但暂未投入生产使用。

> 若要使用，请自行严谨测试，风险自负，本仓库作者和AI概不负责。

## 特性

- **无锁并发** — 入队通过 `Volatile.Read` + `Interlocked.CompareExchange` (CAS) 占位，段满检测为纯读操作，不产生原子写开销
- **批量入队** — `EnqueueRange` 单次 CAS 占位多个槽位，分摊原子操作开销；跨段时自动分批写入
- **分段链表** — 固定大小的段按需分配并链接，段满时 O(1) 创建新段，无需全局暂停或缓冲区迁移
- **原生内存** — 槽位数据通过 `NativeMemory.AllocZeroed` 分配，不产生托管堆分配
- **自动回收** — 段被完全消费后自动释放其原生内存；段元数据由 GC 管理生命周期，确保并发安全
- **False Sharing 防护** — 生产者/消费者热点字段通过缓存行填充隔离，`EnqueuePos` 使用 128 字节独占布局
- **FIFO 保序** — 单生产者视角下严格先入先出，多生产者时每个生产者的消息顺序不变
- **非托管类型约束** — `where T : unmanaged`，支持 `int`、`long`、自定义值类型结构体等

## 快速开始

```csharp
using ConcurrentNativeQueueLibrary;

// 创建队列（默认段大小 32）
using var queue = new ConcurrentNativeQueue<long>();

// 生产者线程入队
queue.Enqueue(42);
queue.Enqueue(100);

// 批量入队
queue.EnqueueRange(new long[] { 200, 300, 400 });

// 查看头部元素（不移除）
if (queue.TryPeek(out long head))
    Console.WriteLine(head); // 42

// 消费者线程出队
if (queue.TryDequeue(out long item))
    Console.WriteLine(item); // 42
```

### 指定段大小

```csharp
// 段大小控制每个段的槽位数量，影响段切换频率与内存粒度
using var queue = new ConcurrentNativeQueue<int>(128);
```

### MPSC 并发模式

```csharp
using var queue = new ConcurrentNativeQueue<long>();

const int producerCount = 4;
const int itemsPerProducer = 50_000;
int totalItems = producerCount * itemsPerProducer;

var barrier = new Barrier(producerCount + 1);
var tasks = new Task[producerCount];

for (int p = 0; p < producerCount; p++)
{
    int pid = p;
    tasks[p] = Task.Run(() =>
    {
        barrier.SignalAndWait();
        for (int i = 0; i < itemsPerProducer; i++)
            queue.Enqueue((long)pid * itemsPerProducer + i);
    });
}

barrier.SignalAndWait();

int consumed = 0;
while (consumed < totalItems)
{
    if (queue.TryDequeue(out long item))
        consumed++;
}

Task.WaitAll(tasks);
```

## API

| 成员 | 说明 |
|---|---|
| `ConcurrentNativeQueue()` | 以默认段大小 32 创建队列 |
| `ConcurrentNativeQueue(int segmentSize)` | 以指定段大小创建队列（最小为 2） |
| `void Enqueue(T item)` | 入队。线程安全，支持多生产者并发调用。段满时自动分配新段 |
| `void EnqueueRange(ReadOnlySpan<T> items)` | 批量入队。单次 CAS 占位多个槽位，跨段时自动分批。线程安全 |
| `bool TryPeek(out T item)` | 查看头部元素但不移除。成功返回 `true`；队列为空返回 `false`。仅限单消费者调用 |
| `bool TryDequeue(out T item)` | 出队。成功返回 `true`；队列为空返回 `false`。仅限单消费者调用 |
| `int Count` | 当前元素数量（并发场景下为近似值） |
| `bool IsEmpty` | 队列是否为空 |
| `void Dispose()` | 释放所有段的原生内存。可多次调用，不会抛出异常 |

## 设计原理

### 分段链表 + 状态标记

队列由固定大小的段组成，每个段包含一个原生内存槽位数组。每个槽位有 `State` 字段标记写入状态：

- `State == 0`：槽位空闲，尚未写入
- `State == 1`：数据已就绪，可读取

生产者通过 `Volatile.Read` 读取当前 `EnqueuePos`，再用 `Interlocked.CompareExchange` (CAS) 原子占位。段满检测为纯读操作，不产生原子写开销。写入数据后通过 `Volatile.Write` 设置 `State = 1` 通知消费者。

`EnqueueRange` 进一步优化：单次 CAS 占位 N 个槽位，先批量写入 Value，经 `Thread.MemoryBarrier()` 后再批量设置 State，将 N 次原子操作分摊为 ⌈N/段剩余容量⌉ 次。

### 段生命周期

段元数据使用托管 `class`，由 GC 跟踪所有引用，确保即使生产者持有旧段引用也不会发生 use-after-free：

1. **段满** — 生产者检测到 `offset >= capacity`，通过 CAS 创建并链接新段到 `Next`，然后推进共享 `_tail` 指针
2. **段消费完毕** — 消费者检测到 `offset >= capacity`，释放当前段的原生内存槽位，前进到 `Next` 段
3. **安全性保证** — 消费者仅在段内所有槽位都被消费后才释放 `Slots`；此时任何持有旧段引用的生产者只会访问托管对象的元数据字段（`EnqueuePos`、`Next` 等），不会触及已释放的原生内存

### 相比旧版环形缓冲区的改进

| 维度 | 旧实现（环形缓冲区） | 新实现（分段链表） |
|---|---|---|
| 每次操作原子指令 | `Interlocked.Inc` + CAS + `Interlocked.Dec` = 3次 | CAS 占位 = 1次（`EnqueueRange` 可 1 次占 N 个） |
| 扩容方式 | Stop-the-world + O(N) 迁移 | 分配新段 O(1)，CAS 链接 |
| 缩容方式 | 显式 TryShrink + O(N) 迁移 | 段消费完后自动释放 |

## 项目结构

```bash
ConcurrentNativeQueue/
├── ConcurrentNativeQueueLibrary/     # 核心库
│   └── ConcurrentNativeQueue.cs
├── ConcurrentNativeQueueDemo/        # MPSC 演示程序（支持 AOT 发布）
│   └── Program.cs
├── ConcurrentNativeQueueBenchmark/   # BenchmarkDotNet 性能基准
│   ├── MpscBenchmark.cs              #   多生产者单消费者并发吞吐量 vs ConcurrentQueue
│   ├── SequentialBenchmark.cs        #   单线程顺序吞吐量 vs ConcurrentQueue（含批量入队对比）
│   ├── BatchEnqueueBenchmark.cs      #   EnqueueRange vs 逐条 Enqueue 吞吐量对比
│   └── SegmentSizeBenchmark.cs       #   不同段大小对吞吐量的影响
└── ConcurrentNativeQueueUnitTest/    # xUnit 单元测试
    └── ConcurrentNativeQueueUnitTest.cs
```

## 运行

### 运行演示

```bash
dotnet run --project ConcurrentNativeQueueDemo
```

### 运行测试

```bash
dotnet test
```

### 运行基准测试

```bash
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

## 要求

- .NET SDK 6.0 或更高。
- 允许 `unsafe` 代码（已在项目文件中启用）

## 许可证

MIT
