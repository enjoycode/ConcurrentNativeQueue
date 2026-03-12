using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ConcurrentNativeQueueBenchmark;

public class Program
{
    /// <summary>
    /// 从环境变量读取 CPU 亲和性掩码，用于将基准测试绑定到大核，避免大小核调度导致的不稳定。
    /// 支持两种方式（优先使用 BENCHMARK_AFFINITY_MASK）：
    /// - BENCHMARK_AFFINITY_MASK: 十六进制掩码，如 0xFFFF 表示仅使用前 16 个逻辑处理器（典型 8P 核+超线程）
    /// - BENCHMARK_BIG_CORES: 大核逻辑处理器数量，如 16 → 掩码为 (1&lt;&lt;16)-1
    /// 例如 Intel 14700K（8P+12E）：P 核通常对应逻辑 0–15，可设置 BENCHMARK_BIG_CORES=16 或 BENCHMARK_AFFINITY_MASK=0xFFFF
    /// </summary>
    static long? GetAffinityMaskFromEnvironment()
    {
        var maskStr = Environment.GetEnvironmentVariable("BENCHMARK_AFFINITY_MASK");
        if (!string.IsNullOrWhiteSpace(maskStr))
        {
            maskStr = maskStr.Trim();
            if (maskStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                && long.TryParse(maskStr.AsSpan(2), System.Globalization.NumberStyles.HexNumber, null, out long hexMask))
                return hexMask;
            if (long.TryParse(maskStr, System.Globalization.NumberStyles.Integer, null, out long decMask))
                return decMask;
        }

        var bigCoresStr = Environment.GetEnvironmentVariable("BENCHMARK_BIG_CORES");
        if (!string.IsNullOrWhiteSpace(bigCoresStr)
            && int.TryParse(bigCoresStr.Trim(), out int n) && n > 0 && n <= 64)
            return (1L << n) - 1;

        return null;
    }

    static Job WithOptionalAffinity(Job job, long? affinityMask) =>
        affinityMask.HasValue ? job.WithAffinity((nint)affinityMask.Value) : job;

    static void Main(string[] args)
    {
        long? affinityMask = GetAffinityMaskFromEnvironment();
        if (affinityMask.HasValue)
            Console.WriteLine($"[Benchmark] 使用 CPU 亲和性掩码: 0x{affinityMask.Value:X} (仅绑定指定逻辑处理器，建议用于大小核 CPU 时固定大核)");

        var config = ManualConfig.CreateMinimumViable()
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.Default)
            .AddExporter(HtmlExporter.Default)
            // 并发 GC
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core60).WithGcConcurrent(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core70).WithGcConcurrent(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core80).WithGcConcurrent(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core90).WithGcConcurrent(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core10_0).WithGcConcurrent(true), affinityMask))
            // 服务器 GC
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core60).WithGcServer(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core70).WithGcServer(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core80).WithGcServer(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core90).WithGcServer(true), affinityMask))
            .AddJob(WithOptionalAffinity(Job.Default.WithRuntime(CoreRuntime.Core10_0).WithGcServer(true), affinityMask));

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
    }
}
