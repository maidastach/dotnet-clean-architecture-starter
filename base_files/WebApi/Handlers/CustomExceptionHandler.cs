using { SolutionName }.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace { SolutionName }.WebApi.Handlers;

public class CustomExceptionHandler(IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Do Logging?

        (int statusCode, string title) = MapException(exception);
        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
        };

        if (!environment.IsProduction())
        {
            problemDetails.Detail = exception.Message;
            problemDetails.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException _ => (StatusCodes.Status400BadRequest, "Invalid request"),
            UnauthorizedAccessException _ => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            ForbiddenAccessException _ => (StatusCodes.Status403Forbidden, "Forbidden"),
            NotFoundException _ => (StatusCodes.Status404NotFound, "The requested resource was not found"),
            TaskCanceledException _ => (StatusCodes.Status504GatewayTimeout, "Request Timeout"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}
