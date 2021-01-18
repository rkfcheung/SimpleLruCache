# SimpleLruCache
Simple Least Recently Used Cache

* [Cache Replacement Policies: Least Recently Used (LRU)](https://en.wikipedia.org/wiki/Cache_replacement_policies#LRU)

## SimpleCache vs MemoryCache

Test Count | SimpleCache (ms) | MemoryCache (ms)
-----------|------------------|-----------------
2,048 | 17 | 74
8,192 | 46 | 87
32,768 | 191 | 289
131,072 | 760 | 1,127

## System Information

Item | Value
-----|------
OS Name	| Microsoft Windows 10 Home
Version	| 10.0.19041 Build 19041
Processor |	Intel(R) Core(TM) i5-8250U CPU @ 1.60GHz, 1800 Mhz, 4 Core(s), 8 Logical Processor(s)
Installed Physical Memory (RAM)	| 8.00 GB
