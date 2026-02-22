using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Users.Commands;

public sealed class CreateUserCommandHandler(
    IMongoRepository<User> users,
    IPasswordHasher passwordHasher,
    IAuditService auditService)
    : IRequestHandler<CreateUserCommand, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = (await users.FilterAsync(
            q => q.Where(u => !u.SoftDelete && u.Email == request.Email),
            cancellationToken)).FirstOrDefault();

        if (existing is not null)
        {
            return ApiResponse<UserDto>.Fail("El email ya existe", "DUPLICATE_EMAIL");
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = passwordHasher.Hash(request.Password),
            RoleId = request.RoleId,
            IsActive = true
        };

        await users.InsertAsync(user, cancellationToken);
        await auditService.WriteAsync("CREATE_USER", "User", user.Id, user.Email, cancellationToken);

        return ApiResponse<UserDto>.Ok(user.Adapt<UserDto>(), "Usuario creado");
    }
}
