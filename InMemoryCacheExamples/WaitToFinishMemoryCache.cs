using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExamples;

public class WaitToFinishMemoryCache<TItem>
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new();
 
    public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
    {
        if (_cache.TryGetValue(key, out TItem cacheEntry))
        {
            return cacheEntry;
        }
        var semaphoreSlim = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
        await semaphoreSlim.WaitAsync();
        try
        {
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await createItem();
                _cache.Set(key, cacheEntry);
            }
        }
        finally
        {
            semaphoreSlim.Release();
        }
        return cacheEntry;
    }
}