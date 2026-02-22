namespace EnterpriseMediaVault.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string plainText);
    bool Verify(string plainText, string hash);
}
