using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace { SolutionName }.Infrastructure.DependencyInjection;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services.RegisterServices(Assembly.GetExecutingAssembly());
    }
}
