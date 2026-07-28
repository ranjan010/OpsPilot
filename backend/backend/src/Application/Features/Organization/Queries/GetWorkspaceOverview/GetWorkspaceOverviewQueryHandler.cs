using Application.Features.Organization.Dtos;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Organization.Queries.GetWorkspaceOverview;

public class GetWorkspaceOverviewQueryHandler : IRequestHandler<GetWorkspaceOverviewQuery, OrganizationOverviewDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;
    private readonly ICurrentUserService _userService;

    public GetWorkspaceOverviewQueryHandler(
        IApplicationDbContext context,
        ICurrentTenantService tenantService,
        ICurrentUserService userService)
    {
        _context = context;
        _tenantService = tenantService;
        _userService = userService;
    }

    public async Task<OrganizationOverviewDto> Handle(GetWorkspaceOverviewQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant)
        {
            throw new InvalidOperationException("Active tenant context is required.");
        }

        var tenantId = _tenantService.TenantId!.Value;

        var org = await _context.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == tenantId && !o.IsDeleted, cancellationToken);

        if (org == null)
        {
            throw new KeyNotFoundException("Organization not found.");
        }

        var memberCount = await _context.OrganizationMembers
            .CountAsync(om => om.OrganizationId == tenantId, cancellationToken);

        var projectCount = await _context.Projects
            .CountAsync(p => p.OrganizationId == tenantId && !p.IsArchived, cancellationToken);

        var openTicketCount = await _context.Tickets
            .CountAsync(t => t.OrganizationId == tenantId && t.Status != TicketStatus.Closed && t.Status != TicketStatus.Resolved, cancellationToken);

        return new OrganizationOverviewDto(
            org.Id,
            org.Name,
            org.Slug,
            _tenantService.UserRole ?? TenantRole.Agent,
            memberCount,
            projectCount,
            openTicketCount,
            org.CreatedAt
        );
    }
}
