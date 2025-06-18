namespace { SolutionName }.Infrastructure.Persistence.Interfaces;

public interface IDbProvider
{
    Task<int?> ExecuteAsync(string proc, object input, DataService dataService, CancellationToken cancellationToken);
    Task<IEnumerable<T>> QueryAsync<T>(string proc, object input, DataService dataService, CancellationToken cancellationToken) where T : class, new();
}