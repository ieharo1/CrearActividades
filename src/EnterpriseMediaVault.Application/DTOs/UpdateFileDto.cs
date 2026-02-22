namespace EnterpriseMediaVault.Application.DTOs;

public sealed class UpdateFileDto
{
    public string Name { get; init; } = string.Empty;
    public Dictionary<string, object> Metadata { get; init; } = new();
}
