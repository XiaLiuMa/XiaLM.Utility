using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace XiaLM.Tool450.source
{
    /// <summary>
    /// 优先级队列
    /// </summary>
    /// <typeparam name="?"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class UtilityPriorityQueue<T> : IProducerConsumerCollection<T>, IDisposable
    {
        // private fields...
        private readonly ConcurrentQueue<T>[] _queues = null;
        private int m_count = 0;
        private int priorityCount = 1;

        // public constructors...
        public UtilityPriorityQueue(int priCount = 1)
        {

            this.priorityCount = priCount;
            _queues = new ConcurrentQueue<T>[priorityCount];
            for (var i = 0; i < priorityCount; i++)
            {
                _queues[i] = new ConcurrentQueue<T>();
            }
        }

        // public properties...
        public int Count
        {
            get
            {
                return m_count;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        private readonly object _syncroot = new object();
        public object SyncRoot
        {
            get
            {
                return _syncroot;
            }
        }



        // private methods...
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo(array as T[], index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private CancellationTokenSource cts;
        public IEnumerable<T> GetConsumingEnumerable() => GetConsumingEnumerable((cts = new CancellationTokenSource()).Token);




        public IEnumerable<T> GetConsumingEnumerable(CancellationToken token)
        {
            lock (_queues)
            {

                T res;
                while ((!token.IsCancellationRequested))
                {
                    if (m_count > 0)
                    {
                        if (TryTake(out res))
                        {
                            yield return res;

                        }
                    }
                    Monitor.Wait(_queues, 50);
                }


            }


        }

        // public methods...
        public void CopyTo(T[] destination, int destStartingIndex)
        {
            if (destination == null)
            {
                throw new ArgumentNullException();
            }
            if (destStartingIndex < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var remaining = destination.Length;
            var temp = this.ToArray();
            for (var i = 0; i < destination.Length && i < temp.Length; i++)
            {
                destination[i] = temp[i];
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = priorityCount - 1; i >= 0; i--)
            {
                foreach (var item in _queues[i])
                {
                    yield return item;
                }
            }
        }
        public T[] ToArray()
        {
            T[] result;
            lock (_queues)
            {
                result = new T[this.Count];
                var index = 0;
                foreach (var q in _queues)
                {
                    if (q.Count > 0)
                    {
                        q.CopyTo(result, index);
                        index += q.Count;
                    }
                }
                return result;
            }
        }

        public bool TryAdd(int Priority, T Value)
        {

            if (Priority < 0)
                Priority = 0;
            if (Priority > priorityCount)
                Priority = priorityCount - 1;
            _queues[Priority].Enqueue(Value);
            Interlocked.Increment(ref m_count);


            return true;




        }

        public bool TryAdd(T Value)
        {
            _queues[0].Enqueue(Value);
            Interlocked.Increment(ref m_count);
            return true;
        }
        public bool TryTake(out T item)
        {
            var success = false;
            lock (_queues)
            {
                for (var i = priorityCount - 1; i >= 0; i--)
                {
                    if (!_queues[i].IsEmpty)
                    {
                        success = _queues[i].TryDequeue(out item);
                        if (success)
                        {
                            Interlocked.Decrement(ref m_count);
                            return true;
                        }
                    }
                }
                item = default(T);
                return false;
            }
        }

        public void Dispose()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}
