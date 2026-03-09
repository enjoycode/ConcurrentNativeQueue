using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConcurrentNativeQueueLibrary;

/// <summary>
/// 一个线程安全的无锁原生队列，用于在并发环境中存储非托管类型。
/// </summary>
/// <remarks>
/// 此结构是一个MPSC多生产者单消费者的CAS无锁队列，旨在为非托管类型的队列提供高效的并发访问。<br/>
/// 允许多个线程安全地入队和出队，而无需外部同步。<br/>
/// 底层使用 <see cref="NativeMemory"/> 分配环形缓冲区，不产生 GC 压力。<br/>
/// 内部容量实现自动伸缩，以适应实际使用需求。
/// </remarks>
/// <typeparam name="T">指定队列中元素的类型。该类型必须是非托管类型，即不包含任何引用类型。</typeparam>
public unsafe struct ConcurrentNativeQueue<T> : IDisposable where T : unmanaged
{
    private const int DefaultCapacity = 16;

    /// <summary>
    /// 环形缓冲区中的槽位，包含用于无锁同步的序列号和实际数据。
    /// 序列号编码了槽位的状态：等于入队位置表示可写，等于入队位置+1表示数据就绪。
    /// </summary>
    private struct Cell
    {
        public long Sequence;
        public T Value;
    }

    private Cell* _buffer;
    private int _bufferMask;
    private long _enqueuePos;
    private long _dequeuePos;
    private int _activeOps;
    private int _resizing;

    public ConcurrentNativeQueue() : this(DefaultCapacity) { }

    public ConcurrentNativeQueue(int capacity)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1);
#else
            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be greater than 0.");
#endif
        capacity = Math.Max(2, (int)BitOperations.RoundUpToPowerOf2((uint)capacity));

        _bufferMask = capacity - 1;
        _buffer = (Cell*)NativeMemory.AllocZeroed((nuint)capacity, (nuint)sizeof(Cell));

        for (int i = 0; i < capacity; i++)
            _buffer[i].Sequence = i;

        _enqueuePos = 0;
        _dequeuePos = 0;
        _activeOps = 0;
        _resizing = 0;
    }

    /// <summary>当前队列中的元素数量（近似值，在并发场景下可能瞬间不精确）。</summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            long count = Volatile.Read(ref _enqueuePos) - Volatile.Read(ref _dequeuePos);
            return (int)Math.Max(count, 0);
        }
    }

    /// <summary>队列是否为空。</summary>
    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _dequeuePos) >= Volatile.Read(ref _enqueuePos);
    }

    /// <summary>当前环形缓冲区容量。</summary>
    public int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _bufferMask) + 1;
    }

    /// <summary>
    /// 将元素入队。此方法是线程安全的，可由多个生产者并发调用。
    /// 当缓冲区已满时会自动扩容。
    /// </summary>
    public void Enqueue(T item)
    {
        SpinWait spin = default;

        while (true)
        {
            while (Volatile.Read(ref _resizing) != 0)
                spin.SpinOnce();

            Interlocked.Increment(ref _activeOps);
            if (Volatile.Read(ref _resizing) != 0)
            {
                Interlocked.Decrement(ref _activeOps);
                continue;
            }

            long pos = Volatile.Read(ref _enqueuePos);
            Cell* cell = _buffer + (pos & _bufferMask);
            long seq = Volatile.Read(ref cell->Sequence);
            long diff = seq - pos;

            if (diff == 0)
            {
                if (Interlocked.CompareExchange(ref _enqueuePos, pos + 1, pos) == pos)
                {
                    cell->Value = item;
                    Volatile.Write(ref cell->Sequence, pos + 1);
                    Interlocked.Decrement(ref _activeOps);
                    return;
                }
            }

            Interlocked.Decrement(ref _activeOps);

            if (diff < 0)
                Grow();
            else
                spin.SpinOnce();
        }
    }

    /// <summary>
    /// 尝试从队列出队一个元素。此方法仅供单个消费者调用。
    /// 出队成功后会检查利用率，当元素数量低于容量的 1/4 时自动收缩缓冲区。
    /// </summary>
    /// <returns>如果成功出队返回 <c>true</c>，队列为空时返回 <c>false</c>。</returns>
    public bool TryDequeue(out T item)
    {
        SpinWait spin = default;

        while (true)
        {
            while (Volatile.Read(ref _resizing) != 0)
                spin.SpinOnce();

            Interlocked.Increment(ref _activeOps);
            if (Volatile.Read(ref _resizing) != 0)
            {
                Interlocked.Decrement(ref _activeOps);
                continue;
            }

            long pos = Volatile.Read(ref _dequeuePos);
            int mask = _bufferMask;
            Cell* cell = _buffer + (pos & mask);
            long seq = Volatile.Read(ref cell->Sequence);

            if (seq == pos + 1)
            {
                item = cell->Value;
                Volatile.Write(ref cell->Sequence, pos + mask + 1);
                Volatile.Write(ref _dequeuePos, pos + 1);
                Interlocked.Decrement(ref _activeOps);
                TryShrink();
                return true;
            }

            Interlocked.Decrement(ref _activeOps);
            item = default;
            return false;
        }
    }

    /// <summary>
    /// 扩容：仅一个线程执行实际扩容，其他线程自旋等待。
    /// 扩容期间通过 _resizing 标志暂停所有入队/出队操作，
    /// 并等待所有正在进行的操作退出后再安全地迁移缓冲区。
    /// </summary>
    private void Grow()
    {
        if (Interlocked.CompareExchange(ref _resizing, 1, 0) != 0)
        {
            SpinWait spin = default;
            while (Volatile.Read(ref _resizing) != 0)
                spin.SpinOnce();
            return;
        }

        try
        {
            SpinWait spin = default;
            while (Volatile.Read(ref _activeOps) > 0)
                spin.SpinOnce();

            int oldCapacity = _bufferMask + 1;
            if (_enqueuePos - _dequeuePos < oldCapacity)
                return;

            MigrateBuffer(oldCapacity * 2);
        }
        finally
        {
            Volatile.Write(ref _resizing, 0);
        }
    }

    /// <summary>
    /// 收缩：当利用率低于 1/4 且容量超过默认值时，将缓冲区缩小一半。
    /// 采用与扩容相同的 _resizing 协调机制，收缩失败时直接放弃（下次出队再试）。
    /// 扩容阈值(100%)与收缩阈值(25%)之间的滞后区间可防止频繁伸缩抖动。
    /// </summary>
    private void TryShrink()
    {
        int capacity = _bufferMask + 1;
        if (capacity <= DefaultCapacity)
            return;

        long count = Volatile.Read(ref _enqueuePos) - Volatile.Read(ref _dequeuePos);
        if (count >= capacity >> 2)
            return;

        if (Interlocked.CompareExchange(ref _resizing, 1, 0) != 0)
            return;

        try
        {
            SpinWait spin = default;
            while (Volatile.Read(ref _activeOps) > 0)
                spin.SpinOnce();

            int oldCapacity = _bufferMask + 1;
            long actualCount = _enqueuePos - _dequeuePos;

            int newCapacity = oldCapacity >> 1;
            if (newCapacity < DefaultCapacity)
                newCapacity = DefaultCapacity;

            if (newCapacity >= oldCapacity || actualCount >= newCapacity)
                return;

            MigrateBuffer(newCapacity);
        }
        finally
        {
            Volatile.Write(ref _resizing, 0);
        }
    }

    /// <summary>
    /// 将当前缓冲区中的有效元素迁移到新容量的缓冲区。
    /// 调用前必须持有 _resizing 标志且 _activeOps 为零。
    /// </summary>
    private void MigrateBuffer(int newCapacity)
    {
        int oldMask = _bufferMask;
        int newMask = newCapacity - 1;
        long head = _dequeuePos;
        long tail = _enqueuePos;

        Cell* newBuffer = (Cell*)NativeMemory.AllocZeroed(
            (nuint)newCapacity, (nuint)sizeof(Cell));

        for (long i = head; i < tail; i++)
        {
            int oldIdx = (int)(i & oldMask);
            int newIdx = (int)(i & newMask);
            newBuffer[newIdx].Value = _buffer[oldIdx].Value;
            newBuffer[newIdx].Sequence = i + 1;
        }

        for (long i = tail; i < head + newCapacity; i++)
        {
            int newIdx = (int)(i & newMask);
            newBuffer[newIdx].Sequence = i;
        }

        NativeMemory.Free(_buffer);
        _buffer = newBuffer;
        _bufferMask = newMask;
    }

    public void Dispose()
    {
        Cell* buffer = _buffer;
        if (buffer != null)
        {
            _buffer = null;
            NativeMemory.Free(buffer);
        }
    }
}
