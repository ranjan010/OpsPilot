using Application.Features.Tickets.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Tickets.Queries.GetTickets;

public class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, List<TicketDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;

    public GetTicketsQueryHandler(IApplicationDbContext context, ICurrentTenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<List<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
        {
            throw new InvalidOperationException("Active tenant context is required.");
        }

        return await _context.Tickets
            .AsNoTracking()
            .Where(t => t.OrganizationId == _tenantService.TenantId.Value && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TicketDto(t.Id, t.Key, t.Title, t.Description, t.Status, t.Priority, null, t.DueDate, t.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
