using BenchmarkDotNet.Attributes;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExamples;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    private readonly IAppCache _appCache = new CachingService();
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    private readonly WaitToFinishMemoryCache<int> _finishMemoryCache = new();

    [Benchmark]
    public int GetOrCreate_MemoryCache() => 
        _memoryCache.GetOrCreate("set", _ => 69);
    [Benchmark]
    public int GetOrCreate_LazyCache() => 
        _appCache.GetOrAdd("set", _ => 69);
    [Benchmark]
    public async Task<int> GetOrCreate_WaitToFinishMemoryCache() => 
        await _finishMemoryCache.GetOrCreate("set", () => Task.FromResult(100));
}