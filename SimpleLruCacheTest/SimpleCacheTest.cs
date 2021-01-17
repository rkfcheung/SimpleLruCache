using NUnit.Framework;
using SimpleLruCache;
using System;

namespace SimpleLruCacheTest
{
    public class SimpleCacheTest
    {
        private SimpleCache cache;

        [SetUp]
        public void Setup()
        {
            cache = new SimpleCache();
        }

        [Test]
        public void TestCountAndClear()
        {
            Assert.AreEqual(0, cache.Count);
            int testCount = 4;
            for (int i = 0; i < testCount; i++)
            {
                cache.Set($"key{i}", $"value{i}");
            }
            Assert.AreEqual(testCount, cache.Count);
            cache.Clear();
            Assert.AreEqual(0L, cache.Count);
        }

        [Test]
        public void TestGetAndSet()
        {
            var now = DateTime.Now;
            cache.Set("now", now);
            Assert.AreEqual(now, (DateTime)cache.Get("now"));

            Assert.IsNull(cache.Get("yesterday"));
        }

        [Test]
        public void TestIgnoreNullValues()
        {
            cache.Set(null, null);
            cache.Set("null", null);
            Assert.AreEqual(cache.Count, 0);
            Assert.IsNull(cache.Get(null));

            cache.Remove(null);
            Assert.AreEqual(cache.Count, 0);
        }

        [Test]
        public void TestInitWithCapacity()
        {
            int capacity = 4;
            var cacheWithCapcity = new SimpleCache(capacity);
            for (int i = 0; i < capacity * 2; i++)
            {
                cacheWithCapcity.Set(i, i * i);
            }
            Assert.AreEqual(cacheWithCapcity.Count, capacity);
        }

        [Test]
        public void TestLeastRecentlyUsed()
        {
            int capacity = 8;
            var lruCache = new SimpleCache(capacity);
            var uuid = Guid.NewGuid();
            var uuid2 = Guid.NewGuid();
            for (int i = 0; i < capacity * 2; i++)
            {
                if (i == 0)
                {
                    lruCache.Set(i, uuid);
                }
                else if (i == 1)
                {
                    lruCache.Set(i, uuid2);

                    Assert.AreEqual(uuid, lruCache.Get(0));
                }
                else
                {
                    Assert.AreEqual(uuid, lruCache.Get(0));
                    Assert.AreEqual(uuid2, lruCache.Get(1));

                    lruCache.Set(i, Guid.NewGuid());
                    Assert.LessOrEqual(lruCache.Count, capacity);

                    if (i >= capacity)
                    {
                        Assert.IsNull(lruCache.Get(i - capacity + 2));
                    }
                }
            }
        }

        [Test]
        public void TestRemove()
        {
            cache.Set("en", "hello");
            cache.Set("fr", "bonjour");
            Assert.AreEqual(2L, cache.Count);

            cache.Remove("fr");
            Assert.AreEqual(1L, cache.Count);
            Assert.AreEqual("hello", cache.Get("en"));
            Assert.IsNull(cache.Get("fr"));
        }

        [Test]
        public void TestUpdate()
        {
            var key = "uuid";
            var uuid = Guid.NewGuid();
            cache.Set(key, uuid);
            Assert.AreEqual(uuid, cache.Get(key));

            cache.Set(key, Guid.NewGuid());
            Assert.AreNotEqual(uuid, cache.Get(key));
            Assert.AreEqual(1L, cache.Count);
        }
    }
}