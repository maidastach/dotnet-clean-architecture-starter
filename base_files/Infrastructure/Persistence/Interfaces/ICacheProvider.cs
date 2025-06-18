using { SolutionName }.Infrastructure.Persistence.Cache;

namespace { SolutionName }.Infrastructure.Persistence.Interfaces;

public interface ICacheProvider
{
    bool TryGet<T>(string cacheKey, out T? value);
    void SetValue(string cacheKey, object value, int ttlMin = default);
}