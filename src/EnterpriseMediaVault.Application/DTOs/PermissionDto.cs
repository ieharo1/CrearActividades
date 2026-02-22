namespace EnterpriseMediaVault.Application.DTOs;

public sealed class PermissionDto
{
    public string Id { get; init; } = string.Empty;
    public string ResourceId { get; init; } = string.Empty;
    public string ResourceType { get; init; } = string.Empty;
    public string SubjectId { get; init; } = string.Empty;
    public string SubjectType { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public bool Allowed { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
