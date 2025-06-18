using System.Data;
using System.Reflection;
using Dapper;
using { SolutionName }.Infrastructure.Attributes;
using { SolutionName }.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace { SolutionName }.Infrastructure.Persistence;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class DbProvider(ISqlConnectionFactory connectionFactory, int retryCount = 1) : IDbProvider
{
    private readonly ISqlConnectionFactory _connectionFactory = connectionFactory;
    private readonly int _retryCount = retryCount;
    private const string _returnValue = "sp_return";

    public async Task<int?> ExecuteAsync(string proc, object input, DataService dataService, CancellationToken cancellationToken)
    {
        Dictionary<string, object?> inputDictionary = [];

        foreach (var property in input.GetType().GetProperties())
        {
            inputDictionary[property.Name.ToLower()] = property.GetValue(input);
            //dynamicParameters.Add(property.Name.ToLower(), property.GetValue(input));
        }

        DynamicParameters dynamicParameters = new(inputDictionary);
        dynamicParameters.Add(_returnValue, null, DbType.Int32, ParameterDirection.Output);


        CommandDefinition commandDefinition = new(
            proc, dynamicParameters, commandTimeout: 20, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken
         );

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            using IDatabase database = _connectionFactory.CreateConnection(dataService);
            Task<int> executeTask = database.ExecuteAsync(commandDefinition);

            // logging/metrics here

            await executeTask;
        }
        catch (NpgsqlException)
        {
            // LOG ERROR
            throw;
            //return null;
        }

        // health tracker
        int? returnValue = dynamicParameters.Get<int?>(_returnValue);
        //string? returnError = dynamicParameters.Get<string?>("error_message");
        //int? returnValue = dynamicParameters.Get<int?>("SpReturnValue");
        return returnValue;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string proc, object input, DataService dataService, CancellationToken cancellationToken)
        where T : class, new()
    {
        // throw if proc or dataservice is null
        DynamicParameters dynamicParameters = new(input);
        dynamicParameters.Add(_returnValue, null, DbType.Int32, ParameterDirection.ReturnValue);
        IEnumerable<T> results;
        int? returnValue;
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            using IDatabase database = _connectionFactory.CreateConnection(dataService);
            CommandDefinition commandDefinition =
                new(commandText: proc, parameters: dynamicParameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            Task<IEnumerable<T>> queryTask = database.QueryAsync<T>(commandDefinition);

            // logging/metrics here

            results = await queryTask;
            returnValue = dynamicParameters.Get<int?>(_returnValue);

            Console.WriteLine($"{proc} returned from db");
            return results;
        }
        catch (NpgsqlException)
        {
            // Log here
            // create custom exception
            // decide to throw exception or null
            throw;
        }
    }
}
