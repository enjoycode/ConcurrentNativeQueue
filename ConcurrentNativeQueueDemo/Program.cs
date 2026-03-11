using System.Text;
using ConcurrentNativeQueueLibrary;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;

const int ProducerCount = 4;
const int ItemsPerProducer = 50_000;
int totalItems = ProducerCount * ItemsPerProducer;

using var queue = new ConcurrentNativeQueue<long>();

Console.WriteLine("=== ConcurrentNativeQueue<long> MPSC 示例 ===");
Console.WriteLine($"生产者: {ProducerCount}，每个生产者: {ItemsPerProducer} 条，总计: {totalItems} 条");
Console.WriteLine();

// --- 批量入队 & TryPeek 演示 ---
Console.WriteLine("--- 批量入队 & TryPeek 演示 ---");
using (var demoQueue = new ConcurrentNativeQueue<long>())
{
    long[] batch = [100, 200, 300, 400, 500];
    demoQueue.EnqueueRange(batch);
    Console.WriteLine($"EnqueueRange 入队 {batch.Length} 个元素，Count = {demoQueue.Count}");

    if (demoQueue.TryPeek(out long head))
        Console.WriteLine($"TryPeek 查看头部: {head}（不移除，Count = {demoQueue.Count}）");

    if (demoQueue.TryDequeue(out long first))
        Console.WriteLine($"TryDequeue 出队: {first}，Count = {demoQueue.Count}");

    if (demoQueue.TryPeek(out long newHead))
        Console.WriteLine($"TryPeek 查看新头部: {newHead}");
}
Console.WriteLine();

var barrier = new Barrier(ProducerCount + 1);
var producerTasks = new Task[ProducerCount];

for (int p = 0; p < ProducerCount; p++)
{
    int pid = p;
    producerTasks[p] = Task.Run(() =>
    {
        barrier.SignalAndWait();
        for (int i = 0; i < ItemsPerProducer; i++)
            queue.Enqueue((long)pid * ItemsPerProducer + i);
    });
}

var consumed = new long[totalItems];
int consumedCount = 0;

var sw = System.Diagnostics.Stopwatch.StartNew();
barrier.SignalAndWait();

while (consumedCount < totalItems)
{
    if (queue.TryDequeue(out long item))
        consumed[consumedCount++] = item;
}

Task.WaitAll(producerTasks);
sw.Stop();

Array.Sort(consumed);
bool allPresent = true;
for (int i = 0; i < totalItems; i++)
{
    if (consumed[i] != i)
    {
        allPresent = false;
        Console.WriteLine($"[错误] 缺失或重复: 索引 {i} 处期望 {i}，实际 {consumed[i]}");
        break;
    }
}

Console.WriteLine($"耗时: {sw.Elapsed.TotalMilliseconds:F2} ms");
Console.WriteLine($"吞吐量: {totalItems / sw.Elapsed.TotalSeconds:N0} ops/sec");
Console.WriteLine($"剩余元素: {queue.Count}");
Console.WriteLine($"数据完整性: {(allPresent ? "通过 ✓" : "失败 ✗")}");
