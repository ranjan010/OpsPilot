using Application.Features.Tickets.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Features.Tickets.Commands.UpdateTicketStatus;

public record UpdateTicketStatusCommand(
    Guid TicketId,
    TicketStatus Status
) : IRequest<TicketDto>;
