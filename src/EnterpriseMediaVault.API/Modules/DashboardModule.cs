using Carter;
using EnterpriseMediaVault.Application.Features.Dashboard.Queries;
using MediatR;

namespace EnterpriseMediaVault.API.Modules;

public sealed class DashboardModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard").RequireAuthorization().WithTags("Dashboard");

        group.MapGet("/summary", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetDashboardQuery(), ct);
            return Results.Ok(result);
        }).RequireAuthorization("AuditorRead");
    }
}
