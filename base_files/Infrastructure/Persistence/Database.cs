using System.Data;
using Dapper;
using { SolutionName }.Infrastructure.Persistence.Interfaces;

namespace { SolutionName }.Infrastructure.Persistence;

public class Database(IDbConnection dbConnection) : IDatabase
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public void Dispose()
    {
        Console.WriteLine("Closing Connection to Db");
        _dbConnection.Close();
        GC.SuppressFinalize(this);
    }

    public async Task<int> ExecuteAsync(CommandDefinition command)
    {
        return await _dbConnection.ExecuteAsync(command);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command)
    {
        return await _dbConnection.QueryAsync<T>(command);
    }
}
