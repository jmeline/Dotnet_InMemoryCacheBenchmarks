# Dotnet_InMemoryCacheBenchmarks

Testing out a couple of in memory caching options with benchmarks

MemoryCache Approach (not thread-safe/atomic)

`4 12 2 11 13 9 10 5 3 7 8 6 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1` 

[LazyCache](https://github.com/alastairtree/LazyCache) Approach

`1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1` 

WaitToFinishMemoryCache Approach (Involves concurrent dictionary and semaphoreSlim)

`1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1` 

## Benchmarks
|                              Method |      Mean |    Error |   StdDev | Allocated |
|------------------------------------ |----------:|---------:|---------:|----------:|
|             GetOrCreate_MemoryCache |  66.61 ns | 1.324 ns | 1.239 ns |         - |
|               GetOrCreate_LazyCache | 150.25 ns | 1.233 ns | 1.030 ns |      96 B |
| GetOrCreate_WaitToFinishMemoryCache | 117.42 ns | 1.580 ns | 1.319 ns |     144 B |
