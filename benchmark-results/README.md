**[English](README.en-US.md)** | 简体中文

# 基准测试结果

本目录用于存放基准测试的报告与结果（如从 `ConcurrentNativeQueueBenchmark/bin/Release/*/BenchmarkDotNet.Artifacts/` 复制过来的 HTML/MD 报告）。

## 本目录下的基准测试结果

| 报告 | 说明 |
|------|------|
| [MpscBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.MpscBenchmark-report.html) | 多生产者单消费者吞吐量：ConcurrentNativeQueue vs ConcurrentQueue vs Channel |
| [MpscBenchmark (MD)](./ConcurrentNativeQueueBenchmark.MpscBenchmark-report-default.md) | 同上，Markdown 版 |
| [SequentialBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.SequentialBenchmark-report.html) | 单线程顺序入队/出队吞吐量 |
| [SequentialBenchmark (MD)](./ConcurrentNativeQueueBenchmark.SequentialBenchmark-report-default.md) | 同上，Markdown 版 |
| [BatchEnqueueBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.BatchEnqueueBenchmark-report.html) | EnqueueRange 与逐条 Enqueue 对比 |
| [BatchEnqueueBenchmark (MD)](./ConcurrentNativeQueueBenchmark.BatchEnqueueBenchmark-report-default.md) | 同上，Markdown 版 |
| [SegmentSizeBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.SegmentSizeBenchmark-report.html) | 不同段大小对吞吐量的影响 |
| [SegmentSizeBenchmark (MD)](./ConcurrentNativeQueueBenchmark.SegmentSizeBenchmark-report-default.md) | 同上，Markdown 版 |

## 运行基准测试

```bash
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

默认输出在对应 TFM 的 `BenchmarkDotNet.Artifacts` 目录下。

### 大小核 CPU：绑定大核以稳定基准

若 CPU 为大小核设计（如 Intel 12/13/14 代），可通过环境变量将基准进程限定在「大核」逻辑处理器上，减少调度到小核带来的波动：

- **BENCHMARK_BIG_CORES**：大核对应的逻辑处理器数量。例如 8 个 P 核 + 超线程 = 16，则设 `16`。
- **BENCHMARK_AFFINITY_MASK**：直接指定十六进制亲和性掩码，如 `0xFFFF` 表示仅使用前 16 个逻辑处理器。

示例（Intel 14700K，P 核通常为逻辑 0–15）：

```bash
# 方式一：按大核数量
set BENCHMARK_BIG_CORES=16
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *

# 方式二：直接掩码
set BENCHMARK_AFFINITY_MASK=0xFFFF
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

不设置上述环境变量时，不应用亲和性，使用系统默认调度。
