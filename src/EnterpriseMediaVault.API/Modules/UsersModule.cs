using Carter;
using EnterpriseMediaVault.Application.Features.Users.Commands;
using EnterpriseMediaVault.Application.Features.Users.Queries;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.Abstractions;
using MediatR;
using EnterpriseMediaVault.Domain.Entities;

namespace EnterpriseMediaVault.API.Modules;

public sealed class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").RequireAuthorization().WithTags("Users");

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetAllUsersQuery(), ct);
            return Results.Ok(result);
        }).RequireAuthorization("AdminOnly");

        group.MapPost("/", async (CreateUserRequestDto dto, ISender sender, IMongoRepository<Role> roles, CancellationToken ct) =>
        {
            var allRoles = await roles.FilterAsync(r => r.Where(x => !x.SoftDelete), ct);
            var role = allRoles.FirstOrDefault(r => r.Name == dto.Role);
            if (role == null)
            {
                return Results.BadRequest(ApiResponse<object>.Fail("Rol no encontrado"));
            }
            
            var command = new CreateUserCommand(dto.Email, dto.Name, dto.Password, role.Id);
            var result = await sender.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization("AdminOnly");

        group.MapPut("/{userId}", async (string userId, UpdateUserRequestDto dto, ISender sender, IMongoRepository<Role> roles, CancellationToken ct) =>
        {
            string? roleId = null;
            if (!string.IsNullOrEmpty(dto.Role))
            {
                var allRoles = await roles.FilterAsync(r => r.Where(x => !x.SoftDelete), ct);
                var role = allRoles.FirstOrDefault(r => r.Name == dto.Role);
                roleId = role?.Id;
            }
            
            var result = await sender.Send(new UpdateUserCommand(userId, dto.Email, dto.Name, null, roleId ?? "", dto.IsActive), ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization("AdminOnly");

        group.MapDelete("/{userId}", async (string userId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteUserCommand(userId), ct);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }).RequireAuthorization("AdminOnly");
    }
}
