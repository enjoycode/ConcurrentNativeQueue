using BenchmarkDotNet.Attributes;
using ConcurrentNativeQueueLibrary;

namespace ConcurrentNativeQueueBenchmark;

/// <summary>
/// 预分配容量 vs 从最小容量自动扩容的开销对比
/// </summary>
[MemoryDiagnoser]
public class GrowthBenchmark
{
    [Params(1_024, 65_536)]
    public int Count;

    private ConcurrentNativeQueue<long> _preAllocated;
    private ConcurrentNativeQueue<long> _autoGrow;

    [IterationSetup]
    public void Setup()
    {
        _preAllocated.Dispose();
        _autoGrow.Dispose();
        _preAllocated = new ConcurrentNativeQueue<long>(Count);
        _autoGrow = new ConcurrentNativeQueue<long>(2);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _preAllocated.Dispose();
        _autoGrow.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void PreAllocated()
    {
        int n = Count;
        for (int i = 0; i < n; i++)
            _preAllocated.Enqueue(i);
    }

    [Benchmark]
    public void AutoGrow()
    {
        int n = Count;
        for (int i = 0; i < n; i++)
            _autoGrow.Enqueue(i);
    }
}