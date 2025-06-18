using { SolutionName }.WebApi.Handlers;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;

namespace { SolutionName }.WebApi.Extensions;

internal static class ServicesExtension
{
    internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCorsPolicy(configuration)
            .AddHsts(HstsConfig)
            .AddMemoryCache()
            .ConfigureAuthentication()
            .AddExceptionHandler<CustomExceptionHandler>()
            .AddApplication()
            .AddInfrastructure()
            .AddOpenApi()
            .AddHealthChecks();

        services.AddEndpointsApiExplorer();

        //services.AddOpenApiDocument((configure, sp) =>
        //{
        //    configure.Title = "CleanArchitecture API";
        //});

        return services;
    }

    private static void HstsConfig(HstsOptions options)
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(60);
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        return services;
    }
}