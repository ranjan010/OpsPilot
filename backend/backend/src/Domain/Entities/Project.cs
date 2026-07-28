using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsArchived { get; set; } = false;

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
