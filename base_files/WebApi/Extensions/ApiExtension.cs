using { SolutionName }.WebApi.Common;
using System.Reflection;

namespace { SolutionName }.WebApi.Extensions;
internal static partial class ApiExtension
{
    internal static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(EndpointGroupBase)));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        app.MapHealthChecks("/api/v1/health");

        return app;
    }

    internal static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;

        return app
            .MapGroup($"/api/v1/{groupName.Replace("Api", "")}")
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithDescription(groupName)
            .WithSummary(groupName)
            .WithOpenApi();
    }


}