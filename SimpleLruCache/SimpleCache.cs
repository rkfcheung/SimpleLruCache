using System;
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

        public event EventHandler<CacheEntryChangedEventArgs> Changed;

        public int Count => _store.Count;

        public SimpleCache() : this(DEFAULT_CAPACITY) { }

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
            if (key == null)
            {
                return null;
            }
            return MoveToFirst(key);
        }

        public void Remove(object key)
        {
            if (key == null)
            {
                return;
            }

            if (_store.TryRemove(key, out _))
            {
                _keys.Remove(key);
            }
            Changed?.Invoke(this, new CacheEntryChangedEventArgs(CacheEventType.Removed, key));
        }

        public void Set(object key, object value)
        {
            if (key == null || value == null)
            {
                return;
            }

            _store[key] = value;
            MoveToFirst(key);
            if (Count > _capacity)
            {
                var last = _keys.Last;
                if (last != null && _store.TryRemove(last.Value, out _))
                {
                    _keys.RemoveLast();
                    Changed?.Invoke(this, new CacheEntryChangedEventArgs(CacheEventType.Removed, last.Value));
                }
            }
            Changed?.Invoke(this, new CacheEntryChangedEventArgs(CacheEventType.CreatedOrUpdated, key));
        }

        private object MoveToFirst(object key)
        {
            if (!_store.ContainsKey(key))
            {
                return null;
            }

            _keys.Remove(key); // O(1) https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.remove
            _keys.AddFirst(key);
            return _store[key];
        }
    }
}
