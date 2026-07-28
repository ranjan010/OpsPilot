using Application.Features.Tickets.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Features.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(
    string Title,
    string Description,
    TicketPriority Priority,
    Guid? ProjectId,
    Guid? AssigneeId,
    DateTime? DueDate
) : IRequest<TicketDto>;
