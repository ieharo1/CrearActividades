using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Auth.Commands;

public sealed class LoginCommandHandler(
    IMongoRepository<User> users,
    IMongoRepository<Role> roles,
    IMongoRepository<RefreshToken> refreshTokens,
    IPasswordHasher hasher,
    IJwtTokenService jwt)
    : IRequestHandler<LoginCommand, ApiResponse<AuthTokenDto>>
{
    public async Task<ApiResponse<AuthTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = (await users.FilterAsync(q => q.Where(x => x.Email == request.Email && !x.SoftDelete), cancellationToken)).FirstOrDefault();
        if (user is null || !hasher.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<AuthTokenDto>.Fail("Credenciales inválidas", "INVALID_CREDENTIALS");
        }

        var role = (await roles.GetByIdAsync(user.RoleId, cancellationToken))?.Name ?? "Employee";
        var (token, expires) = jwt.GenerateAccessToken(user, role, "default");
        var refreshTokenValue = jwt.GenerateRefreshToken();

        await refreshTokens.InsertAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return ApiResponse<AuthTokenDto>.Ok(new AuthTokenDto
        {
            AccessToken = token,
            ExpiresAtUtc = expires,
            RefreshToken = refreshTokenValue,
            Role = role,
            User = new UserInfo
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email
            }
        }, "Login exitoso");
    }
}
