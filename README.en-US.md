English | **[简体中文](README.md)**

# ConcurrentNativeQueue\<T\>

A high-performance, lock-free MPSC (Multi-Producer, Single-Consumer) native queue built on a CAS ring buffer for .NET 6+.

Memory is allocated via `NativeMemory` with **zero GC pressure**. The buffer automatically grows and shrinks — no manual capacity management required.

> Note: This project was entirely written using `Claude Opus 4.6 High Thinking`.

> Although it passes unit tests, it has not yet been used in production.

> Use at your own risk after thorough testing. Neither the repository author nor the AI assumes any responsibility.

## Features

- **Lock-free concurrency** — CAS-based MPSC queue; multiple threads can enqueue without locks
- **Native memory** — Ring buffer allocated via `NativeMemory.AllocZeroed`; no managed heap allocations
- **Auto-scaling** — Automatically doubles capacity when full; shrinks to half when utilization drops below 25% (never below the default capacity of 16)
- **Jitter suppression** — Hysteresis between the grow threshold (100%) and shrink threshold (25%) prevents resize thrashing
- **FIFO ordering** — Strict FIFO from a single producer's perspective; per-producer ordering is preserved across multiple producers
- **Unmanaged type constraint** — `where T : unmanaged`; supports `int`, `long`, custom value-type structs, etc.

## Quick Start

```csharp
using ConcurrentNativeQueueLibrary;

// Create a queue (default initial capacity: 16)
using var queue = new ConcurrentNativeQueue<long>();

// Producer thread enqueues
queue.Enqueue(42);
queue.Enqueue(100);

// Consumer thread dequeues
if (queue.TryDequeue(out long item))
    Console.WriteLine(item); // 42
```

### Specifying Initial Capacity

```csharp
// Capacity is rounded up to the nearest power of two
using var queue = new ConcurrentNativeQueue<int>(1000); // actual capacity: 1024
```

### MPSC Concurrent Pattern

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

| Member | Description |
|---|---|
| `ConcurrentNativeQueue()` | Creates a queue with the default capacity of 16 |
| `ConcurrentNativeQueue(int capacity)` | Creates a queue with the specified capacity (rounded up to a power of two, minimum 2) |
| `void Enqueue(T item)` | Enqueues an item. Thread-safe for multiple concurrent producers. Automatically grows when full |
| `bool TryDequeue(out T item)` | Dequeues an item. Returns `true` on success; `false` if the queue is empty. Single-consumer only |
| `int Count` | Current number of elements (approximate under concurrency) |
| `bool IsEmpty` | Whether the queue is empty |
| `int Capacity` | Current ring buffer capacity |
| `void Dispose()` | Frees the underlying native memory. Safe to call multiple times |

## Design

### Ring Buffer + Sequence Numbers

Each slot contains a `Sequence` field that encodes its state:

- `Sequence == enqueue position` — slot is free and writable
- `Sequence == enqueue position + 1` — data is ready to be consumed

Producers compete for `_enqueuePos` via CAS. On success, the producer writes data and advances the sequence number. The consumer checks whether the sequence number indicates readiness before reading.

### Auto-Grow

Growth is triggered when the buffer is full (`diff < 0`):

1. A single winning thread sets the `_resizing` flag
2. Waits for all in-flight operations (`_activeOps`) to drain
3. Allocates a new buffer at double the capacity and migrates live elements
4. Frees the old buffer and clears the `_resizing` flag

### Auto-Shrink

After each dequeue, utilization is checked. When the element count falls below capacity / 4 and the capacity exceeds the default, the buffer is halved. If another thread is already resizing, the shrink attempt is abandoned and retried on the next dequeue.

## Project Structure

```
ConcurrentNativeQueue/
├── ConcurrentNativeQueueLibrary/     # Core library
│   └── ConcurrentNativeQueue.cs
├── ConcurrentNativeQueueDemo/        # MPSC demo app (AOT-publishable)
│   └── Program.cs
├── ConcurrentNativeQueueBenchmark/   # BenchmarkDotNet benchmarks
│   ├── SequentialBenchmark.cs        #   Single-thread sequential throughput vs ConcurrentQueue
│   ├── MpscBenchmark.cs              #   MPSC concurrent throughput vs ConcurrentQueue
│   └── GrowthBenchmark.cs            #   Pre-allocated vs auto-grow overhead
└── ConcurrentNativeQueueUnitTest/    # xUnit unit tests
    └── ConcurrentNativeQueueUnitTest.cs
```

## Running

### Run the Demo

```bash
dotnet run --project ConcurrentNativeQueueDemo
```

### Run Tests

```bash
dotnet test
```

### Run Benchmarks

```bash
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

## Requirements

- .NET SDK 6.0 or later.
- `unsafe` code must be enabled (already configured in the project files)

## License

MIT
