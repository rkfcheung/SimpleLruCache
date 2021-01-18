using NUnit.Framework;
using SimpleLruCache;
using System;
using System.Collections.Generic;

namespace SimpleLruCacheTest
{
    public class CacheEntryChangedEventArgsTest
    {
        private int testCount = 8;
        private Dictionary<CacheEventType, List<Guid>> events;
        private List<Guid> keys;

        [SetUp]
        public void Setup()
        {
            events = new Dictionary<CacheEventType, List<Guid>>
            {
                [CacheEventType.CreatedOrUpdated] = new List<Guid>(),
                [CacheEventType.Removed] = new List<Guid>()
            };

            keys = new List<Guid>();
            for (int i = 0; i < testCount; i++)
            {
                keys.Add(Guid.NewGuid());
            }
        }

        [Test]
        public void TestCreatedOrUpdated()
        {
            var cache = new SimpleCache();
            cache.Changed += new EventHandler<CacheEntryChangedEventArgs>(ChangedHandler);
            for (int i = 0; i < testCount; i++)
            {
                cache.Set(keys[i], DateTime.Now);
            }

            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(keys[i], events[CacheEventType.CreatedOrUpdated][i]);
            }
        }

        [Test]
        public void TestRemoved()
        {
            var cache = new SimpleCache(1);
            cache.Changed += new EventHandler<CacheEntryChangedEventArgs>(ChangedHandler);
            for (int i = 0; i < testCount; i++)
            {
                cache.Set(keys[i], DateTime.Now);
            }

            for (int i = 0; i < testCount - 1; i++)
            {
                Assert.AreEqual(keys[i], events[CacheEventType.Removed][i]);
            }
            Assert.IsFalse(events[CacheEventType.Removed].Contains(keys[testCount - 1]));
        }

        private void ChangedHandler(object sender, CacheEntryChangedEventArgs e)
        {
            var key = (Guid)e.Key;
            if (e.EventType == CacheEventType.CreatedOrUpdated)
            {
                events[CacheEventType.CreatedOrUpdated].Add(key);
            }
            else if (e.EventType == CacheEventType.Removed)
            {
                events[CacheEventType.Removed].Add(key);
            }
        }
    }
}
