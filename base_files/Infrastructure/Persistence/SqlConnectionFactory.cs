using { SolutionName }.Infrastructure.Attributes;
using { SolutionName }.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace { SolutionName }.Infrastructure.Persistence;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration = configuration;

    public IDatabase CreateConnection(DataService dataService)
    {
        string connectionString = dataService switch
        {
            DataService.Default => _configuration.GetConnectionString("Default") ?? throw new ArgumentException($"Invalid connection string for {DataService.Default}"),
            _ => throw new InvalidOperationException()
        };

        var sqlConnection = new NpgsqlConnection(connectionString);

        return new Database(sqlConnection);
    }
}
