using Microsoft.AspNetCore.Mvc;

namespace { SolutionName }.WebApi.Extensions;

internal static class ConfigurationsExtension
{
    internal static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure <{ SolutionName }
        Configs > (configuration.GetSection(nameof({ SolutionName }
        Configs)));

        return services;
    }
}
