namespace { SolutionName }.Infrastructure.Persistence.Interfaces;

public interface ISqlConnectionFactory
{
    IDatabase CreateConnection(DataService dataService);
}
