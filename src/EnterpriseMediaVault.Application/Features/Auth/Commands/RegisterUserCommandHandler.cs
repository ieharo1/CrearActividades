using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Auth.Commands;

public sealed class RegisterUserCommandHandler(
    IMongoRepository<User> users,
    IMongoRepository<Role> roles,
    IMongoRepository<RefreshToken> refreshTokens,
    IPasswordHasher hasher,
    IJwtTokenService jwt,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterUserCommand, ApiResponse<AuthTokenDto>>
{
    public async Task<ApiResponse<AuthTokenDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = (await users.FilterAsync(q => q.Where(x => x.Email == request.Email && !x.SoftDelete), cancellationToken)).FirstOrDefault();
        if (existing is not null)
        {
            return ApiResponse<AuthTokenDto>.Fail("Usuario ya existe", "EMAIL_ALREADY_EXISTS");
        }

        var role = (await roles.FilterAsync(q => q.Where(r => r.Name == request.RoleName), cancellationToken)).FirstOrDefault()
            ?? (await roles.FilterAsync(q => q.Where(r => r.Name == "Employee"), cancellationToken)).First();

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = hasher.Hash(request.Password),
            RoleId = role.Id
        };

        var (token, expires) = jwt.GenerateAccessToken(user, role.Name, "default");
        var refreshTokenValue = jwt.GenerateRefreshToken();

        await unitOfWork.ExecuteAsync(async () =>
        {
            await users.InsertAsync(user, cancellationToken);
            await refreshTokens.InsertAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
            }, cancellationToken);
        }, cancellationToken);

        return ApiResponse<AuthTokenDto>.Ok(new AuthTokenDto
        {
            AccessToken = token,
            ExpiresAtUtc = expires,
            RefreshToken = refreshTokenValue,
            Role = role.Name
        }, "Usuario registrado");
    }
}
