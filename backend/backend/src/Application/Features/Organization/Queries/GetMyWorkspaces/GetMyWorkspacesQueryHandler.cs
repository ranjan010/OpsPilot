using Application.Features.Organization.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Organization.Queries.GetMyWorkspaces;

public class GetMyWorkspacesQueryHandler : IRequestHandler<GetMyWorkspacesQuery, List<OrganizationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyWorkspacesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<OrganizationDto>> Handle(GetMyWorkspacesQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = _currentUserService.UserId.Value;

        return await _context.OrganizationMembers
            .AsNoTracking()
            .Where(om => om.UserId == userId && !om.Organization.IsDeleted)
            .Select(om => new OrganizationDto(
                om.Organization.Id,
                om.Organization.Name,
                om.Organization.Slug,
                om.Role,
                om.JoinedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
