using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Users.Commands;

public sealed class UpdateUserCommandHandler(
    IMongoRepository<User> users,
    IPasswordHasher passwordHasher,
    IAuditService auditService)
    : IRequestHandler<UpdateUserCommand, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<UserDto>.Fail("Usuario no encontrado", "NOT_FOUND");
        }

        var emailExists = (await users.FilterAsync(
            q => q.Where(u => !u.SoftDelete && u.Email == request.Email && u.Id != request.UserId),
            cancellationToken)).FirstOrDefault();

        if (emailExists is not null)
        {
            return ApiResponse<UserDto>.Fail("El email ya existe", "DUPLICATE_EMAIL");
        }

        user.Email = request.Email;
        user.FullName = request.FullName;
        user.RoleId = request.RoleId;
        user.IsActive = request.IsActive;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = passwordHasher.Hash(request.Password);
        }

        await users.ReplaceAsync(user, cancellationToken);
        await auditService.WriteAsync("UPDATE_USER", "User", user.Id, user.Email, cancellationToken);

        return ApiResponse<UserDto>.Ok(user.Adapt<UserDto>(), "Usuario actualizado");
    }
}
