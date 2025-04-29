using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class FixedLengthQueue<T>
        {
            #region private fields

            private readonly Queue<T> _queue;
            private readonly int _maxSize;

            #endregion

            #region properties

            public int Count => _queue.Count;

            #endregion

            #region construction

            public FixedLengthQueue(int maxSize)
            {
                _maxSize = maxSize;
                _queue = new Queue<T>(maxSize);
            }

            #endregion


            public void Enqueue(T item)
            {
                if (_queue.Count >= _maxSize)
                {
                    _queue.Dequeue();
                }
                _queue.Enqueue(item);
            }

            public T Dequeue()
            {
                return _queue.Dequeue();
            }

            public T Peek()
            {
                return _queue.Peek();
            }

            public void Clear()
            {
                _queue.Clear();
            }

            public T[] ToArray()
            {
                return _queue.ToArray();
            }

            public List<T> ToList()
            {
                return _queue.ToList();
            }
        }
    }
}