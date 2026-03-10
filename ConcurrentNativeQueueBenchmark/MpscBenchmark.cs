using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using ConcurrentNativeQueueLibrary;

namespace ConcurrentNativeQueueBenchmark;

/// <summary>
/// MPSC 多生产者单消费者吞吐量：NativeQueue vs ConcurrentQueue，
/// 变化生产者数量观察扩展性。
/// </summary>
[MemoryDiagnoser]
public class MpscBenchmark
{
    [Params(1, 2, 4)]
    public int ProducerCount;

    private const int TotalItems = 100_000;

    private ConcurrentNativeQueue<long> _nativeQueue;
    private ConcurrentQueue<long> _managedQueue = null!;

    [IterationSetup]
    public void Setup()
    {
        _nativeQueue.Dispose();
        _nativeQueue = new ConcurrentNativeQueue<long>();
        _managedQueue = new ConcurrentQueue<long>();
    }

    [GlobalCleanup]
    public void Cleanup() => _nativeQueue.Dispose();

    [Benchmark(Baseline = true)]
    public int ConcurrentQueue()
    {
        int itemsPerProducer = TotalItems / ProducerCount;
        var barrier = new Barrier(ProducerCount + 1);
        var tasks = new Task[ProducerCount];
        var q = _managedQueue;

        for (int p = 0; p < ProducerCount; p++)
        {
            int pid = p;
            tasks[p] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                for (int i = 0; i < itemsPerProducer; i++)
                    q.Enqueue((long)pid * itemsPerProducer + i);
            });
        }

        int consumed = 0;
        barrier.SignalAndWait();
        while (consumed < TotalItems)
        {
            if (q.TryDequeue(out _))
                consumed++;
        }

        Task.WaitAll(tasks);
        return consumed;
    }

    [Benchmark]
    public int NativeQueue()
    {
        int itemsPerProducer = TotalItems / ProducerCount;
        var barrier = new Barrier(ProducerCount + 1);
        var tasks = new Task[ProducerCount];

        for (int p = 0; p < ProducerCount; p++)
        {
            int pid = p;
            tasks[p] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                for (int i = 0; i < itemsPerProducer; i++)
                    _nativeQueue.Enqueue((long)pid * itemsPerProducer + i);
            });
        }

        int consumed = 0;
        barrier.SignalAndWait();
        while (consumed < TotalItems)
        {
            if (_nativeQueue.TryDequeue(out _))
                consumed++;
        }

        Task.WaitAll(tasks);
        return consumed;
    }
}
