using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Dashboard.Queries;

public sealed class GetDashboardQueryHandler(
    IMongoRepository<FileDocument> files,
    IMongoRepository<Folder> folders,
    IMongoRepository<AuditLog> auditLogs,
    ICurrentUserService currentUser)
    : IRequestHandler<GetDashboardQuery, ApiResponse<DashboardDto>>
{
    public async Task<ApiResponse<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var tenantFiles = await files.FilterAsync(q => q.Where(f => f.TenantId == currentUser.TenantId && !f.SoftDelete), cancellationToken);
        var tenantFolders = await folders.FilterAsync(q => q.Where(f => f.TenantId == currentUser.TenantId && !f.SoftDelete), cancellationToken);
        var tenantAudits = await auditLogs.FilterAsync(q => q.Where(a => a.TenantId == currentUser.TenantId && !a.SoftDelete), cancellationToken);

        var mostDownloaded = tenantFiles
            .OrderByDescending(f => f.DownloadCount)
            .Take(10)
            .Select(f => new HotFileDto { FileId = f.Id, Name = f.Name, Downloads = f.DownloadCount })
            .ToList();

        var byUser = tenantAudits
            .GroupBy(a => a.UserId)
            .Select(g => new UserActivityDto { UserId = g.Key, Events = g.LongCount() })
            .OrderByDescending(x => x.Events)
            .Take(10)
            .ToList();

        var recent = tenantAudits
            .OrderByDescending(a => a.CreatedAtUtc)
            .Take(20)
            .Select(a => new AuditDto
            {
                UserId = a.UserId,
                Action = a.Action,
                ResourceType = a.ResourceType,
                AtUtc = a.CreatedAtUtc
            })
            .ToList();

        return ApiResponse<DashboardDto>.Ok(new DashboardDto
        {
            TotalFiles = tenantFiles.LongCount(),
            TotalFolders = tenantFolders.LongCount(),
            UsedBytes = tenantFiles.Sum(f => f.Size),
            MostDownloaded = mostDownloaded,
            ActivityByUser = byUser,
            RecentAudits = recent
        });
    }
}
