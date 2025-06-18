using { SolutionName }.Application.Interfaces;

namespace { SolutionName }.WebApi.Apis.Test;

internal partial class TestApi
{
    internal async Task<IResult> TestMethod(CancellationToken cancellationToken)
    {
        return Results.Ok();
    }

    internal async Task<IResult> TestMethodId(string id, CancellationToken cancellationToken)
    {
        return Results.Ok();
    }
}
