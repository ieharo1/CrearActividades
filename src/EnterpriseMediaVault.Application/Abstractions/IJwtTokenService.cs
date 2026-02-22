using EnterpriseMediaVault.Domain.Entities;

namespace EnterpriseMediaVault.Application.Abstractions;

public interface IJwtTokenService
{
    (string token, DateTime expiresAtUtc) GenerateAccessToken(User user, string roleName, string tenantId);
    string GenerateRefreshToken();
}
