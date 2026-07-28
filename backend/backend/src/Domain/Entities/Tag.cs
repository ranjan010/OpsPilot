using Domain.Common;

namespace Domain.Entities;

public class Tag : BaseEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#3B82F6";

    public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
}
