using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleLruCache
{
    class Program
    {
        class SimpleKey
        {
            Guid guid = Guid.NewGuid();
            DateTime dateTime = DateTime.Now;

            public override string ToString()
            {
                return $"SimpleKey({guid}, {dateTime})";
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("## SimpleCache vs MemoryCache");
            Console.WriteLine();
            Console.WriteLine("Test Count | SimpleCache (ms) | MemoryCache (ms)");
            Console.WriteLine("-----------|------------------|-----------------");
            int maxTestCount = 2 << 16;
            for (int testCount = 2 << 10; testCount <= maxTestCount; testCount <<= 2)
            {
                int capacity = 128;
                var cacheOptions = new MemoryCacheOptions { SizeLimit = capacity };
                var entryOptions = new MemoryCacheEntryOptions { Size = 1 };
                var client = new HttpClient();
                var requestUrl = "https://httpbin.org/cache";
                var response = await client.GetAsync(requestUrl);
                SimpleKey key = null;
                var stopWatch = new Stopwatch();

                stopWatch.Start();
                var simpleCache = SimpleCache.Build().SpecifyCapacity(capacity);
                for (int i = 0; i < testCount; i++)
                {
                    simpleCache.Get(key);
                    key = new SimpleKey();
                    simpleCache.Set(key, response);
                    Debug.Assert(simpleCache.Count <= capacity);
                }
                stopWatch.Stop();
                var timeForSimpleCache = stopWatch.ElapsedMilliseconds;

                stopWatch.Reset();
                stopWatch.Start();
                var memoryCache = new MemoryCache(cacheOptions);
                for (int i = 0; i < testCount; i++)
                {
                    memoryCache.Get(key);
                    key = new SimpleKey();
                    memoryCache.Set(key, response, entryOptions);
                    Debug.Assert(memoryCache.Count <= capacity);
                }
                stopWatch.Stop();
                var timeForMemoryCache = stopWatch.ElapsedMilliseconds;

                Console.WriteLine($"{testCount} | {timeForSimpleCache} | {timeForMemoryCache}");
            }
        }
    }
}
