# SimpleLruCache
Simple Least Recently Used Cache

* [Cache Replacement Policies: Least Recently Used (LRU)](https://en.wikipedia.org/wiki/Cache_replacement_policies#LRU)

## SimpleCache vs MemoryCache

 Test Count | SimpleCache (ms) | MemoryCache (ms)
------------|------------------|-----------------
2,048 | 17 | 74
8,192 | 46 | 87
32,768 | 191 | 289
131,072 | 760 | 1,127
