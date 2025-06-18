using Dapper;

namespace { SolutionName }.Infrastructure.Persistence.Interfaces;

public interface IDatabase : IDisposable
{
    Task<int> ExecuteAsync(CommandDefinition command);
    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command);
    //Task<int> QueryMultipleAsync();
}
