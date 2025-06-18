using { SolutionName }.Infrastructure.Persistence.Cache;

namespace { SolutionName }.Infrastructure.Persistence.Interfaces;

public interface IDataAccess
{
    Task<int?> ExecuteAsync(string proc, object input, CancellationToken cancellationToken, DataService dataService = DataService.Default);
    Task<IEnumerable<T>> QueryAsync<T>(
        string proc, object input, CancellationToken cancellationToken,
        CacheOptions? cacheOptions = default, DataService dataService = DataService.Default
        )
        where T : class, new();
}
