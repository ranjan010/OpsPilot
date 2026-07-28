using Domain.Common;

namespace Domain.Entities;

public class TicketComment : BaseEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;

    public Guid AuthorId { get; set; }
    public Users Author { get; set; } = default!;

    public string Content { get; set; } = string.Empty;
}
