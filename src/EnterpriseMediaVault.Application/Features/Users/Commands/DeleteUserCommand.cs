using EnterpriseMediaVault.Application.Common;
using MediatR;
using EnterpriseMediaVault.Domain.Entities;
using EnterpriseMediaVault.Application.Abstractions;

namespace EnterpriseMediaVault.Application.Features.Users.Commands;

public sealed record DeleteUserCommand(string UserId) : IRequest<ApiResponse<bool>>;

public sealed class DeleteUserCommandHandler(
    IMongoRepository<User> users,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteUserCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(request.UserId, ct);
        
        if (user == null)
        {
            return ApiResponse<bool>.Fail("Usuario no encontrado");
        }

        user.SoftDelete = true;
        await users.ReplaceAsync(user, ct);

        return ApiResponse<bool>.Ok(true, "Usuario eliminado");
    }
}
