English | **[简体中文](README.md)**

# Benchmark Results

This directory holds benchmark reports and results (e.g. HTML/MD reports copied from `ConcurrentNativeQueueBenchmark/bin/Release/*/BenchmarkDotNet.Artifacts/`).

## Benchmark Results in This Directory

| Report | Description |
|--------|-------------|
| [MpscBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.MpscBenchmark-report.html) | MPSC throughput: ConcurrentNativeQueue vs ConcurrentQueue vs Channel |
| [MpscBenchmark (MD)](./ConcurrentNativeQueueBenchmark.MpscBenchmark-report-default.md) | Same, Markdown version |
| [SequentialBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.SequentialBenchmark-report.html) | Single-thread sequential enqueue/dequeue throughput |
| [SequentialBenchmark (MD)](./ConcurrentNativeQueueBenchmark.SequentialBenchmark-report-default.md) | Same, Markdown version |
| [BatchEnqueueBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.BatchEnqueueBenchmark-report.html) | EnqueueRange vs per-item Enqueue comparison |
| [BatchEnqueueBenchmark (MD)](./ConcurrentNativeQueueBenchmark.BatchEnqueueBenchmark-report-default.md) | Same, Markdown version |
| [SegmentSizeBenchmark (HTML)](./ConcurrentNativeQueueBenchmark.SegmentSizeBenchmark-report.html) | Impact of segment size on throughput |
| [SegmentSizeBenchmark (MD)](./ConcurrentNativeQueueBenchmark.SegmentSizeBenchmark-report-default.md) | Same, Markdown version |

## Running Benchmarks

```bash
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

Output is written under the corresponding TFM’s `BenchmarkDotNet.Artifacts` directory.

### Big.LITTLE CPUs: Pinning to Big Cores for Stable Results

On hybrid (big.LITTLE) CPUs (e.g. Intel 12th/13th/14th gen), you can restrict the benchmark process to “big core” logical processors via environment variables to reduce variance from being scheduled onto little cores:

- **BENCHMARK_BIG_CORES** — Number of logical processors for big cores. E.g. 8 P-cores with hyper-threading = 16, so set `16`.
- **BENCHMARK_AFFINITY_MASK** — Hex affinity mask. E.g. `0xFFFF` means use only the first 16 logical processors.

Example (Intel 14700K; P-cores are typically logical 0–15):

```bash
# Option 1: by big-core count
set BENCHMARK_BIG_CORES=16
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *

# Option 2: explicit mask
set BENCHMARK_AFFINITY_MASK=0xFFFF
dotnet run --project ConcurrentNativeQueueBenchmark -c Release -- --filter *
```

If neither variable is set, no affinity is applied and the system default scheduling is used.
