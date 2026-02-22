using EnterpriseMediaVault.Application.Abstractions;

namespace EnterpriseMediaVault.Infrastructure.Auth;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string plainText) => BCrypt.Net.BCrypt.HashPassword(plainText);

    public bool Verify(string plainText, string hash) => BCrypt.Net.BCrypt.Verify(plainText, hash);
}
