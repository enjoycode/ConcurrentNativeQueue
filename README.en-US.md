English | **[简体中文](README.md)**

# ConcurrentNativeQueue\<T\>

A high-performance, lock-free MPSC (Multi-Producer, Single-Consumer) native queue built on a segmented linked-list design for .NET 6+.

All memory (segment headers + slot arrays) is allocated via `NativeMemory` with **zero GC pressure and zero managed heap allocations**. New segments are allocated on demand with exponentially growing capacity; slot arrays are freed immediately after consumption, segment headers are reclaimed on `Dispose`.

> Note: This project was entirely written using `Claude Opus 4.6 High Thinking`.

> Although it passes unit tests, it has not yet been used in production.

> Use at your own risk after thorough testing. Neither the repository author nor the AI assumes any responsibility.

## Features

- **Lock-free concurrency** — Enqueue claims a slot via `Volatile.Read` + `Interlocked.CompareExchange` (CAS); full-segment detection is a pure read with no atomic write overhead
- **Batch enqueue** — `EnqueueRange` claims multiple slots in a single CAS, amortizing atomic operation cost; automatically splits across segments
- **Segmented linked list** — Segments are allocated on demand with capacity that grows exponentially (up to 1M cap); O(1) new segment creation, no global pauses or buffer migration
- **Fully native memory** — Both segment headers and slot arrays are allocated via `NativeMemory`; zero managed heap allocations; `ConcurrentNativeQueue<T>` itself is an unmanaged type
- **Two-phase reclamation** — Slot arrays are freed immediately by the consumer after full consumption; segment headers are deferred to `Dispose` (producers may hold stale pointers — the core lifetime challenge in lock-free native design)
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
// Specify the initial segment size (subsequent segments double in capacity, up to 1M)
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
| `ConcurrentNativeQueue(int segmentSize)` | Creates a queue with the specified initial segment size (minimum 2; subsequent segments grow exponentially up to 1M) |
| `void Enqueue(T item)` | Enqueues an item. Thread-safe for multiple concurrent producers. Allocates a new segment when full |
| `void EnqueueRange(ReadOnlySpan<T> items)` | Batch enqueue. Claims multiple slots in a single CAS; auto-splits across segments. Thread-safe |
| `bool TryPeek(out T item)` | Peeks at the head element without removing it. Returns `true` on success; `false` if empty. Single-consumer only |
| `bool TryDequeue(out T item)` | Dequeues an item. Returns `true` on success; `false` if the queue is empty. Single-consumer only |
| `int Count` | Current number of elements (approximate under concurrency) |
| `bool IsEmpty` | Whether the queue is empty |
| `void Dispose()` | Frees native memory for all segments. Safe to call multiple times |

## Design

### Segmented Linked List + State Flags

The queue consists of native segments, each containing a `NativeMemory`-allocated slot array. Segment capacity grows exponentially from the initial value (×2 up to 1M cap), reducing segment transition frequency. Each slot has a `State` field indicating write status:

- `State == 0` — slot is empty, not yet written
- `State == 1` — data is ready for consumption

Producers read the current `EnqueuePos` via `Volatile.Read`, then atomically claim a slot using `Interlocked.CompareExchange` (CAS). Full-segment detection is a pure read with no atomic write overhead. After writing data, the producer sets `State = 1` via `Volatile.Write` to signal the consumer.

`EnqueueRange` further optimizes this: a single CAS claims N slots at once, values are written in bulk, followed by a `Thread.MemoryBarrier()`, then all States are set — reducing N atomic operations to ⌈N / remaining segment capacity⌉.

### Segment Lifecycle (Fully Native, Two-Phase Reclamation)

Both segment headers and slot arrays are allocated via `NativeMemory` — no managed objects involved.
The core lifetime challenge: **a producer may be preempted at any moment while holding a stale pointer to an old segment**, so segment headers cannot be freed immediately after consumption.

1. **Segment full** — A producer detects `offset >= capacity`, creates a new segment (doubled capacity) via CAS on `Next`, then advances the shared `_tail` pointer. If the CAS loses (another producer created one first), the redundant segment is freed immediately (it was never published, so no concurrent access risk)
2. **Segment consumed** — The consumer detects `offset >= capacity`, **frees the slot array immediately** (safe: all `State == 1` means all producers have finished writing), and advances to `Next`. The segment header is retained
3. **Dispose** — Walks the entire chain from `_origin` (the first segment) to free all segment headers and any remaining slot arrays
4. **Header overhead** — Each segment header is ~160 bytes. Exponential growth keeps the header count logarithmic: processing 1 billion items ≈ 1,000 headers ≈ 200KB, entirely negligible

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
