var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddConfigurations(configuration)
    .AddServices(configuration);

//builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

await builder
    .Build()
    .UseMiddlewares()
    .MapEndpoints()
    .RunAsync();

//app.MapFallbackToFile("index.html");
