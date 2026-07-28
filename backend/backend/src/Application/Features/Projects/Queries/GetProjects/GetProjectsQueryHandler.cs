using Application.Features.Projects.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;

    public GetProjectsQueryHandler(IApplicationDbContext context, ICurrentTenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
        {
            throw new InvalidOperationException("Active tenant context is required.");
        }

        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.OrganizationId == _tenantService.TenantId.Value)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto(p.Id, p.Name, p.Key, p.Description, p.IsArchived, p.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
