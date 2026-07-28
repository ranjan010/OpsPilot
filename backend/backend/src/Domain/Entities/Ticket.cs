using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Ticket : BaseEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;

    public Guid? AssigneeId { get; set; }
    public Users? Assignee { get; set; }

    public Guid ReporterId { get; set; }
    public Users Reporter { get; set; } = default!;

    public DateTime? DueDate { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
}
