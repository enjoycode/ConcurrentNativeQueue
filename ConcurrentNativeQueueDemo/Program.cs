using System.Text;
using ConcurrentNativeQueueLibrary;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.Unicode;

const int ProducerCount = 4;
const int ItemsPerProducer = 50_000;
int totalItems = ProducerCount * ItemsPerProducer;

using var queue = new ConcurrentNativeQueue<long>();

Console.WriteLine($"=== ConcurrentNativeQueue<long> MPSC 示例 ===");
Console.WriteLine($"生产者: {ProducerCount}，每个生产者: {ItemsPerProducer} 条，总计: {totalItems} 条");
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
Console.WriteLine($"最终容量: {queue.Capacity}，剩余元素: {queue.Count}");
Console.WriteLine($"数据完整性: {(allPresent ? "通过 ✓" : "失败 ✗")}");
