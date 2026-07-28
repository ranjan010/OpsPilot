using Application.Features.Tickets.Dtos;
using MediatR;

namespace Application.Features.Tickets.Queries.GetTickets;

public record GetTicketsQuery : IRequest<List<TicketDto>>;
