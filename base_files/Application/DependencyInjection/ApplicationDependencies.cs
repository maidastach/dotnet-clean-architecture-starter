using { SolutionName }.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace { SolutionName }.Application.DependencyInjection;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.RegisterServices(Assembly.GetExecutingAssembly());
    }
}
