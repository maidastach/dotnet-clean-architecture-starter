namespace { SolutionName }.WebApi.Cors;

public sealed class CorsConfigs
{
    public required string PolicyName { get; init; }
    public required string[] AllowedOrigins { get; init; } = [];
}
