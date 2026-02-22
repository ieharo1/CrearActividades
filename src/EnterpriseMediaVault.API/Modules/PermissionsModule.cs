using Carter;
using EnterpriseMediaVault.Application.Features.Permissions.Commands;
using MediatR;

namespace EnterpriseMediaVault.API.Modules;

public sealed class PermissionsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/permissions").RequireAuthorization("AdminOnly").WithTags("Permissions");

        group.MapPost("/grant", async (GrantPermissionCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        });
    }
}
