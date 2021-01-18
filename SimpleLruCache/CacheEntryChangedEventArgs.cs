using System;

namespace SimpleLruCache
{
    public class CacheEntryChangedEventArgs : EventArgs
    {
        public CacheEventType EventType;
        public object Key;

        public CacheEntryChangedEventArgs(CacheEventType eventType, object key)
        {
            EventType = eventType;
            Key = key;
        }
    }
}
