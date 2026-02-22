using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Auth.Commands;

public sealed class RefreshTokenCommandHandler(
    IMongoRepository<RefreshToken> refreshTokens,
    IMongoRepository<User> users,
    IMongoRepository<Role> roles,
    IJwtTokenService jwt)
    : IRequestHandler<RefreshTokenCommand, ApiResponse<AuthTokenDto>>
{
    public async Task<ApiResponse<AuthTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenEntity = (await refreshTokens.FilterAsync(q => q.Where(x => x.Token == request.RefreshToken && !x.SoftDelete), cancellationToken)).FirstOrDefault();
        if (tokenEntity is null || !tokenEntity.IsActive)
        {
            return ApiResponse<AuthTokenDto>.Fail("Refresh token inválido", "INVALID_REFRESH_TOKEN");
        }

        var user = await users.GetByIdAsync(tokenEntity.UserId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<AuthTokenDto>.Fail("Usuario no encontrado", "USER_NOT_FOUND");
        }

        var roleName = (await roles.GetByIdAsync(user.RoleId, cancellationToken))?.Name ?? "Employee";
        var (token, expiresAtUtc) = jwt.GenerateAccessToken(user, roleName, "default");
        var newRefreshToken = jwt.GenerateRefreshToken();

        tokenEntity.RevokedAtUtc = DateTime.UtcNow;
        tokenEntity.ReplacedByToken = newRefreshToken;

        await refreshTokens.ReplaceAsync(tokenEntity, cancellationToken);
        await refreshTokens.InsertAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return ApiResponse<AuthTokenDto>.Ok(new AuthTokenDto
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            RefreshToken = newRefreshToken,
            Role = roleName
        }, "Token refrescado");
    }
}
