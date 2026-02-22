using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Dashboard.Queries;

public sealed record GetDashboardQuery : IRequest<ApiResponse<DashboardDto>>;
