using Domain.Enums;

namespace Application.Features.Tickets.Dtos;

public record TicketDto(
    Guid Id,
    string Key,
    string Title,
    string Description,
    TicketStatus Status,
    TicketPriority Priority,
    string? AssigneeName,
    DateTime? DueDate,
    DateTime CreatedAt
);
