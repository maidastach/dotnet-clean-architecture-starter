namespace { SolutionName }.WebApi.Cors;

internal static class CorsPolicy
{
    private static string? _policyName;
    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsConfigs = configuration.GetSection(nameof(CorsConfigs)).Get<CorsConfigs>();
        var allowedOrigins = corsConfigs?.AllowedOrigins;
        _policyName = corsConfigs?.PolicyName;

        if (string.IsNullOrWhiteSpace(_policyName) || allowedOrigins == null || allowedOrigins.Length < 1)
        {
            throw new ArgumentException("Invalid Cors configs");
        }

        services.AddCors(options =>
        {
            options.AddPolicy(
                _policyName,
                builder =>
                {
                    builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // TODO
                }
            );
        });
        return services;
    }

    internal static WebApplication UseCorsPolicy(this WebApplication app)
    {
        if (string.IsNullOrWhiteSpace(_policyName))
        {
            throw new ArgumentException("Invalid Cors configs");
        }
        app.UseCors(_policyName);
        return app;
    }
}
