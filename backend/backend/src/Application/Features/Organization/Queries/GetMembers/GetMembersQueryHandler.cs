using Application.Features.Organization.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Organization.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, List<MemberDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;

    public GetMembersQueryHandler(IApplicationDbContext context, ICurrentTenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<List<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
            throw new InvalidOperationException("Active tenant context is required.");

        var tenantId = _tenantService.TenantId.Value;

        return await _context.OrganizationMembers
            .AsNoTracking()
            .Where(om => om.OrganizationId == tenantId)
            .OrderBy(om => om.Role)
            .ThenBy(om => om.JoinedAt)
            .Select(om => new MemberDto(
                om.UserId,
                om.User.Email,
                om.User.FirstName,
                om.User.LastName,
                om.Role,
                om.JoinedAt))
            .ToListAsync(cancellationToken);
    }
}
