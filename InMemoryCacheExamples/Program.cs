// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using InMemoryCacheExamples;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

BenchmarkRunner.Run<Benchmarks>();
// Console.WriteLine("\nMemoryCache Approach");
// MemoryCacheApproach();
// Console.WriteLine("\nLazyCache Approach");
// LazyCacheApproach();
// Console.WriteLine("\nMemoryCacheThreadSafeDictionary Approach");
// await MemoryCacheThreadSafeDictionary();

/*
MemoryCache Approach (not thread-safe/atomic)
4 12 2 11 13 9 10 5 3 7 8 6 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 
LazyCache Approach
1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 
MemoryCacheThreadSafeDictionary Approach
1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 

Benchmarks
|                              Method |      Mean |    Error |   StdDev | Allocated |
|------------------------------------ |----------:|---------:|---------:|----------:|
|             GetOrCreate_MemoryCache |  66.61 ns | 1.324 ns | 1.239 ns |         - |
|               GetOrCreate_LazyCache | 150.25 ns | 1.233 ns | 1.030 ns |      96 B |
| GetOrCreate_WaitToFinishMemoryCache | 117.42 ns | 1.580 ns | 1.319 ns |     144 B |

*/

void MemoryCacheApproach()
{
    var cache = new MemoryCache(new MemoryCacheOptions());
    var counter = 0;

    Parallel.ForEach(Enumerable.Range(1, 50), _ =>
    {
        var cachedItem = cache.GetOrCreate("key", _ => Interlocked.Increment(ref counter));
        Console.Write($"{cachedItem} ");
    });
}

void LazyCacheApproach()
{
    var cache = new CachingService();
    var counter = 0;

    Parallel.ForEach(Enumerable.Range(1, 50), _ =>
    {
        var cachedItem = cache.GetOrAdd("key", _ => Interlocked.Increment(ref counter));
        Console.Write($"{cachedItem} ");
    });
}

async Task MemoryCacheThreadSafeDictionary()
{
    var cache = new WaitToFinishMemoryCache<int>();
    int counter = 0;
    await Parallel.ForEachAsync(Enumerable.Range(1, 50), async (_, __) =>
    {
        var cachedItem = await cache.GetOrCreate("key", () => Task.FromResult(Interlocked.Increment(ref counter)));
        Console.Write($"{cachedItem} ");
    });
}