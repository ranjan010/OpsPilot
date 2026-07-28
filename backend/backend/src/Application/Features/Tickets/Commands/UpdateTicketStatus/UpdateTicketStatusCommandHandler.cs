using Application.Features.Tickets.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Tickets.Commands.UpdateTicketStatus;

public class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, TicketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;

    public UpdateTicketStatusCommandHandler(IApplicationDbContext context, ICurrentTenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<TicketDto> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
            throw new InvalidOperationException("Active tenant context is required.");

        var tenantId = _tenantService.TenantId.Value;

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == request.TicketId && t.OrganizationId == tenantId && !t.IsDeleted, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException($"Ticket '{request.TicketId}' was not found in the active workspace.");

        ticket.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        var assigneeName = ticket.AssigneeId.HasValue
            ? await _context.Users
                .Where(u => u.Id == ticket.AssigneeId.Value)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefaultAsync(cancellationToken)
            : null;

        return new TicketDto(
            ticket.Id,
            ticket.Key,
            ticket.Title,
            ticket.Description,
            ticket.Status,
            ticket.Priority,
            assigneeName,
            ticket.DueDate,
            ticket.CreatedAt);
    }
}
