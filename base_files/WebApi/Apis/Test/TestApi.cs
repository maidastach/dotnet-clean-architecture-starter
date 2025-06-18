using { SolutionName }.WebApi.Common;

namespace { SolutionName }.WebApi.Apis.Test;

internal partial class TestApi : EndpointGroupBase
{
    internal override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(TestMethod)
            .MapGet(TestMethodId, "{id}");

        app.MapGroup(this)
            //.RequireAuthorization()
            //.MapPost(CreateListing);
            .MapPost(async () => await Task.Run(() => "Hi, I am a POST Api"));
    }
}

