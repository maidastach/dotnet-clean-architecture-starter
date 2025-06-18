using System.Diagnostics.CodeAnalysis;

namespace { SolutionName }.WebApi.Extensions;

internal static class IEndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    internal static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler)//, [StringSyntax("Route")] string pattern = "")
    {
        builder.MapPost("", handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    internal static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    internal static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }
}
