using Application.Features.Tickets.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;
    private readonly ICurrentUserService _currentUserService;

    public CreateTicketCommandHandler(
        IApplicationDbContext context,
        ICurrentTenantService tenantService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _tenantService = tenantService;
        _currentUserService = currentUserService;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
            throw new InvalidOperationException("Active tenant context is required.");

        if (!_currentUserService.UserId.HasValue)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var tenantId = _tenantService.TenantId.Value;
        var reporterId = _currentUserService.UserId.Value;

        Guid resolvedProjectId;

        if (request.ProjectId.HasValue)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId.Value && p.OrganizationId == tenantId, cancellationToken);

            if (project is null)
                throw new KeyNotFoundException("The selected project does not belong to the active workspace.");

            resolvedProjectId = project.Id;
        }
        else
        {
            var fallbackProject = await _context.Projects
                .Where(p => p.OrganizationId == tenantId && !p.IsArchived)
                .OrderBy(p => p.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (fallbackProject is null)
                throw new InvalidOperationException("No active project found in this workspace. Please create a project first.");

            resolvedProjectId = fallbackProject.Id;
        }

        var ticket = new Ticket
        {
            OrganizationId = tenantId,
            ProjectId = resolvedProjectId,
            Key = $"OPS-{DateTime.UtcNow:yyMMddHHmmss}",
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority,
            Status = TicketStatus.Open,
            DueDate = request.DueDate,
            ReporterId = reporterId
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return new TicketDto(ticket.Id, ticket.Key, ticket.Title, ticket.Description, ticket.Status, ticket.Priority, null, ticket.DueDate, ticket.CreatedAt);
    }
}
