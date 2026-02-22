namespace EnterpriseMediaVault.Application.DTOs;

public sealed class DashboardDto
{
    public long TotalFiles { get; init; }
    public long TotalFolders { get; init; }
    public long UsedBytes { get; init; }
    public IReadOnlyCollection<HotFileDto> MostDownloaded { get; init; } = Array.Empty<HotFileDto>();
    public IReadOnlyCollection<UserActivityDto> ActivityByUser { get; init; } = Array.Empty<UserActivityDto>();
    public IReadOnlyCollection<AuditDto> RecentAudits { get; init; } = Array.Empty<AuditDto>();
}

public sealed class HotFileDto
{
    public string FileId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public long Downloads { get; init; }
}

public sealed class UserActivityDto
{
    public string UserId { get; init; } = string.Empty;
    public long Events { get; init; }
}

public sealed class AuditDto
{
    public string UserId { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string ResourceType { get; init; } = string.Empty;
    public DateTime AtUtc { get; init; }
}
