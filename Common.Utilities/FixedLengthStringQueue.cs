using System.Collections.Generic;
using System.Linq;

namespace IngameScript
{
    partial class Program
    {
        public class FixedLengthStringQueue
        {
            #region private fields

            private readonly Queue<string> _queue;
            private readonly int _maxSize;

            #endregion

            #region properties

            public int Count => _queue.Count;

            #endregion

            #region construction

            public FixedLengthStringQueue(int maxSize)
            {
                _maxSize = maxSize;
                _queue = new Queue<string>(maxSize);
            }

            #endregion


            public void Enqueue(string item)
            {
                if (_queue.Count >= _maxSize)
                {
                    _queue.Dequeue();
                }
                _queue.Enqueue(item);
            }

            public string Dequeue()
            {
                return _queue.Dequeue();
            }

            public string Peek()
            {
                return _queue.Peek();
            }

            public void Clear()
            {
                _queue.Clear();
            }

            public string[] ToArray()
            {
                return _queue.ToArray();
            }

            public List<string> ToList()
            {
                return _queue.ToList();
            }
        }
    }
}