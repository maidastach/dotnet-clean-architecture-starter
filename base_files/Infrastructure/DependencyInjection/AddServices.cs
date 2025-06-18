using { SolutionName }.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace { SolutionName }.Infrastructure.DependencyInjection;

public static class AddServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, Assembly assembly)
    {
        var assemblyServices = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<ServiceLifetimeAttribute>() != null)
            .Select(s => new
            {
                Lifetime = s.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime ?? throw new ArgumentNullException(s.Name),
                Service = s,
                Interface = s.GetInterface($"I{s.Name}") ?? throw new ArgumentNullException(s.Name)
            });

        foreach (var service in assemblyServices)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Transient:
                    services.AddTransient(service.Interface, service.Service);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(service.Interface, service.Service);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(service.Interface, service.Service);
                    break;
                default:
                    throw new ArgumentException("Invalid service {ServiceName}", service.Service.Name);
            }
        }

        return services;
    }
}
