using Carter;
using EnterpriseMediaVault.Application.Features.Auth.Commands;
using MediatR;

namespace EnterpriseMediaVault.API.Modules;

public sealed class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterUserCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).AllowAnonymous();

        group.MapPost("/login", async (LoginCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (!result.Success)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(result);
        }).AllowAnonymous();

        group.MapPost("/refresh", async (RefreshTokenCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.Unauthorized();
        }).AllowAnonymous();
    }
}
