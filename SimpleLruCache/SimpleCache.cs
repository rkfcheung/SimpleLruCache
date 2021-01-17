using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimpleLruCache
{
    public class SimpleCache : ICache
    {
        private static readonly int DEFAULT_CAPACITY = 64;
        private readonly int _capacity;
        private readonly ConcurrentDictionary<object, object> _store;
        private readonly LinkedList<object> _keys;

        public int Count => _store.Count;

        public SimpleCache() : this(DEFAULT_CAPACITY)
        {
        }

        public SimpleCache(int capacity)
        {
            _capacity = capacity;
            _store = new ConcurrentDictionary<object, object>();
            _keys = new LinkedList<object>();
        }

        public void Clear()
        {
            _store.Clear();
            _keys.Clear();
        }

        public object Get(object key)
        {
            return MoveFirst(key);
        }

        public void Remove(object key)
        {
            if (_store.TryRemove(key, out _))
            {
                _keys.Remove(key);
            }
        }

        public void Set(object key, object value)
        {
            _store[key] = value;
            MoveFirst(key);
            if (Count > _capacity)
            {
                var last = _keys.Last;
                if (last != null && _store.TryRemove(last.Value, out _))
                {
                    _keys.RemoveLast();
                }
            }
        }

        private object MoveFirst(object key)
        {
            object value = null;
            if (_store.ContainsKey(key))
            {
                value = _store[key];
                _keys.Remove(key);
                _keys.AddFirst(key);
            }
            return value;
        }
    }
}
