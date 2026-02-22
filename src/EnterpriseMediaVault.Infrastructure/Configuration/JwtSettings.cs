namespace EnterpriseMediaVault.Infrastructure.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = "EnterpriseMediaVault";
    public string Audience { get; init; } = "EnterpriseMediaVault.Client";
    public string Secret { get; init; } = "change-this-super-secret-key-at-least-32-bytes";
    public int ExpirationMinutes { get; init; } = 60;
}
