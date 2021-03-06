# SimpleLruCache
Simple Least Recently Used Cache

* [Cache Replacement Policies: Least Recently Used (LRU)](https://en.wikipedia.org/wiki/Cache_replacement_policies#LRU)

## Usage

```cmd
dotnet test
dotnet build
dotnet run --project SimpleLruCache
```

```csharp
var capacity = 128; // Default: 64
var cache = SimpleCache.Build().SpecifyCapacity(capacity);
cache.Changed += new EventHandler<CacheEntryChangedEventArgs>(ChangedHandler);

// ...

private void ChangedHandler(object sender, CacheEntryChangedEventArgs e)
{
    var key = e.Key;
    if (e.EventType == CacheEventType.CreatedOrUpdated)
    {
        // ... Handle CacheEventType events ...
    }
    else if (e.EventType == CacheEventType.Removed)
    {
        // .. Handle CacheEventType.Removed events
    }
}
```

## SimpleCache vs MemoryCache

* [Test Code](SimpleLruCache/Program.cs)

Test Count | SimpleCache (ms) | MemoryCache (ms)
-----------|------------------|-----------------
2,048 | 14 | 23
8,192 | 49 | 74
32,768 | 186 | 259
131,072 | 733 | 1,031

## Test Machine System Information

Item | Value
-----|------
OS Name	| Microsoft Windows 10 Home
Version	| 10.0.19041 Build 19041
Processor |	Intel(R) Core(TM) i5-8250U CPU @ 1.60GHz, 1800 Mhz, 4 Core(s), 8 Logical Processor(s)
Installed Physical Memory (RAM)	| 8.00 GB
