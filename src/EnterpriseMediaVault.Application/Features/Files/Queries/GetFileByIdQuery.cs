using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed record GetFileByIdQuery(string FileId) : IRequest<ApiResponse<FileDto>>;
