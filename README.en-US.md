English | **[简体中文](README.md)**

# ConcurrentNativeQueue\<T\>

A high-performance, lock-free MPSC (Multi-Producer, Single-Consumer) native queue built on a segmented linked-list design for .NET 6+.

Slot data is allocated via `NativeMemory` with **zero GC pressure**. New segments are allocated on demand when full and automatically freed after consumption — no manual capacity management required.

> Note: This project was entirely written using `Claude Opus 4.6 High Thinking`.

> Although it passes unit tests, it has not yet been used in production.

> Use at your own risk after thorough testing. Neither the repository author nor the AI assumes any responsibility.

## Features

- **Lock-free concurrency** — Enqueue claims a slot via `Volatile.Read` + `Interlocked.CompareExchange` (CAS); full-segment detection is a pure read with no atomic write overhead
- **Batch enqueue** — `EnqueueRange` claims multiple slots in a single CAS, amortizing atomic operation cost; automatically splits across segments
- **Segmented linked list** — Fixed-size segments are allocated and linked on demand; O(1) new segment creation when full, no global pauses or buffer migration
- **Native memory** — Slot data allocated via `NativeMemory.AllocZeroed`; no managed heap allocations
- **Automatic reclamation** — Consumed segments automatically free their native memory; segment metadata is GC-managed to ensure concurrent safety
- **False sharing prevention** — Producer/consumer hot fields are isolated via cache-line padding; `EnqueuePos` uses a 128-byte exclusive layout
- **FIFO ordering** — Strict FIFO from a single producer's perspective; per-producer ordering is preserved across multiple producers
- **Unmanaged type constraint** — `where T : unmanaged`; supports `int`, `long`, custom value-type structs, etc.

## Quick Start

```csharp
using ConcurrentNativeQueueLibrary;

// Create a queue (default segment size: 32)
using var queue = new ConcurrentNativeQueue<long>();

// Producer thread enqueues
queue.Enqueue(42);
queue.Enqueue(100);

// Batch enqueue
queue.EnqueueRange(new long[] { 200, 300, 400 });

// Peek at the head element (without removing)
if (queue.TryPeek(out long head))
    Console.WriteLine(head); // 42

// Consumer thread dequeues
if (queue.TryDequeue(out long item))
    Console.WriteLine(item); // 42
```

### Specifying Segment Size

```csharp
// Segment size controls the number of slots per segment,
// affecting segment transition frequency and memory granularity
using var queue = new ConcurrentNativeQueue<int>(128);
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
| `ConcurrentNativeQueue()` | Creates a queue with the default segment size of 32 |
| `ConcurrentNativeQueue(int segmentSize)` | Creates a queue with the specified segment size (minimum 2) |
| `void Enqueue(T item)` | Enqueues an item. Thread-safe for multiple concurrent producers. Allocates a new segment when full |
| `void EnqueueRange(ReadOnlySpan<T> items)` | Batch enqueue. Claims multiple slots in a single CAS; auto-splits across segments. Thread-safe |
| `bool TryPeek(out T item)` | Peeks at the head element without removing it. Returns `true` on success; `false` if empty. Single-consumer only |
| `bool TryDequeue(out T item)` | Dequeues an item. Returns `true` on success; `false` if the queue is empty. Single-consumer only |
| `int Count` | Current number of elements (approximate under concurrency) |
| `bool IsEmpty` | Whether the queue is empty |
| `void Dispose()` | Frees native memory for all segments. Safe to call multiple times |

## Design

### Segmented Linked List + State Flags

The queue consists of fixed-size segments, each containing a native memory slot array. Each slot has a `State` field indicating write status:

- `State == 0` — slot is empty, not yet written
- `State == 1` — data is ready for consumption

Producers read the current `EnqueuePos` via `Volatile.Read`, then atomically claim a slot using `Interlocked.CompareExchange` (CAS). Full-segment detection is a pure read with no atomic write overhead. After writing data, the producer sets `State = 1` via `Volatile.Write` to signal the consumer.

`EnqueueRange` further optimizes this: a single CAS claims N slots at once, values are written in bulk, followed by a `Thread.MemoryBarrier()`, then all States are set — reducing N atomic operations to ⌈N / remaining segment capacity⌉.

### Segment Lifecycle

Segment metadata uses a managed `class`, with the GC tracking all references to ensure that even if a producer holds a stale reference to an old segment, use-after-free cannot occur:

1. **Segment full** — A producer detects `offset >= capacity`, creates a new segment via CAS on `Next`, then advances the shared `_tail` pointer
2. **Segment consumed** — The consumer detects `offset >= capacity`, frees the current segment's native slot memory, and advances to the `Next` segment
3. **Safety guarantee** — The consumer only frees `Slots` after all slots in the segment have been consumed; any producer still holding a stale reference only accesses managed metadata fields (`EnqueuePos`, `Next`, etc.) and never touches freed native memory

### Improvements over the Previous Ring Buffer Design

| Aspect | Old (Ring Buffer) | New (Segmented Linked List) |
|---|---|---|
| Atomic ops per operation | `Interlocked.Inc` + CAS + `Interlocked.Dec` = 3 | CAS to claim slot = 1 (`EnqueueRange`: 1 CAS for N slots) |
| Growth strategy | Stop-the-world + O(N) migration | Allocate new segment O(1), CAS link |
| Shrink strategy | Explicit TryShrink + O(N) migration | Automatic free after consumption |

## Project Structure

```bash
ConcurrentNativeQueue/
├── ConcurrentNativeQueueLibrary/     # Core library
│   └── ConcurrentNativeQueue.cs
├── ConcurrentNativeQueueDemo/        # MPSC demo app (AOT-publishable)
│   └── Program.cs
├── ConcurrentNativeQueueBenchmark/   # BenchmarkDotNet benchmarks
│   ├── MpscBenchmark.cs              #   MPSC concurrent throughput vs ConcurrentQueue
│   ├── SequentialBenchmark.cs        #   Single-thread sequential throughput vs ConcurrentQueue (incl. batch)
│   ├── BatchEnqueueBenchmark.cs      #   EnqueueRange vs per-item Enqueue throughput
│   └── SegmentSizeBenchmark.cs       #   Segment size impact on throughput
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
