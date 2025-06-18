using { SolutionName }.Infrastructure.Attributes;
using { SolutionName }.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace { SolutionName }.Infrastructure.Persistence.Cache;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class CacheProvider(IMemoryCache cache) : ICacheProvider
{
    private readonly IMemoryCache _cache = cache;
    private const int TTL_Minutes = 30;

    //_ = options.RegisterPostEvictionCallback(OnPostEviction);

    public bool TryGet<T>(string cacheKey, out T? value)
    {
        return _cache.TryGetValue(cacheKey, out value);
    }

    public void SetValue(string cacheKey, object value, int ttlMin)
    {
        if (ttlMin == 0)
        {
            ttlMin = TTL_Minutes;
        }
        _cache.Set(cacheKey, value, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(ttlMin));
    }
}
