using System.Data;
using Dapper;
using { SolutionName }.Infrastructure.Attributes;
using { SolutionName }.Infrastructure.Persistence.Cache;
using { SolutionName }.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace { SolutionName }.Infrastructure.Persistence;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class DataAccess(IDbProvider dbProvider, ICacheProvider cacheProvider) : IDataAccess
{
    private readonly IDbProvider _dbProvider = dbProvider;
    private readonly ICacheProvider _cacheProvider = cacheProvider;

    public async Task<int?> ExecuteAsync(string proc, object input, CancellationToken cancellationToken, DataService dataService = DataService.Default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // TODO: implement caching here
        return await _dbProvider.ExecuteAsync(proc, input, dataService, cancellationToken);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string proc, object input, CancellationToken cancellationToken,
        CacheOptions? cacheOptions = default, DataService dataService = DataService.Default
        )
        where T : class, new()
    {
        // throw if proc or dataservice is null
        cancellationToken.ThrowIfCancellationRequested();

        //if (_cacheProvider.TryGet(cacheOptions ?? new() { CacheKey = proc }, out IEnumerable<T> value))
        //{
        //    Console.WriteLine($"{proc} returned from cache");
        //    return value!;
        //}

        IEnumerable<T> results = await _dbProvider.QueryAsync<T>(proc, input, dataService, cancellationToken);
        // check returnvalue
        _cacheProvider.SetValue(proc, results);
        return results;
    }
}
