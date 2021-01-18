using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimpleLruCache
{
    public class SimpleCache : ICache
    {
        private static readonly int DEFAULT_CAPACITY = 64;
        private int _capacity;
        private bool _capacitySpecified = false;
        private readonly ConcurrentDictionary<object, object> _store;
        private readonly LinkedList<object> _keys;

        public event EventHandler<CacheEntryChangedEventArgs> Changed;

        public int Count => _store.Count;

        private SimpleCache() : this(DEFAULT_CAPACITY) { }

        private SimpleCache(int capacity)
        {
            _capacity = capacity;
            _store = new ConcurrentDictionary<object, object>();
            _keys = new LinkedList<object>();
        }

        public static SimpleCache Build()
        {
            return new SimpleCache();
        }

        public SimpleCache SpecifyCapacity(int capacity)
        {
            if (!_capacitySpecified && Count == 0)
            {
                _capacity = capacity;
                _capacitySpecified = true;
            }
            else
            {
                throw new InvalidOperationException($"Capacity was already specified to {_capacity}!");
            }
            return this;
        }

        public void Clear()
        {
            _store.Clear();
            lock (_keys)
            {
                _keys.Clear();
            }
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
                lock (_keys)
                {
                    _keys.Remove(key);
                }
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
                lock (_keys)
                {
                    var last = _keys.Last;
                    if (last != null && _store.TryRemove(last.Value, out _))
                    {
                        _keys.RemoveLast();
                        Changed?.Invoke(this, new CacheEntryChangedEventArgs(CacheEventType.Removed, last.Value));
                    }
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

            lock (_keys)
            {
                _keys.Remove(key); // O(1) https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.remove
                _keys.AddFirst(key);
            }
            return _store[key];
        }
    }
}
