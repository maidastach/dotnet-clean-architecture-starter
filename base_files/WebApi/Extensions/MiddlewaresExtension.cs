namespace { SolutionName }.WebApi.Extensions;

internal static class MiddlewaresExtension
{
    internal static WebApplication UseMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        else
        {
            app.MapOpenApi();
        }

        app
            .UseCorsPolicy()
            .UseHttpsRedirection()
            .UseExceptionHandler(options => { });

        // app.UseStaticFiles();

        return app;
    }
}
