namespace Domain.Entities;

public class TicketTag
{
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}
