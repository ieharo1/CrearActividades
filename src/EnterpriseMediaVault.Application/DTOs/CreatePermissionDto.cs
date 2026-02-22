namespace EnterpriseMediaVault.Application.DTOs;

public sealed class CreatePermissionDto
{
    public string SubjectId { get; init; } = string.Empty;
    public string SubjectType { get; init; } = "User";
    public string ResourceId { get; init; } = string.Empty;
    public string ResourceType { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public bool Allowed { get; init; } = true;
}
