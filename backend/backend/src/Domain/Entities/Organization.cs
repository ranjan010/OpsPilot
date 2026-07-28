using Domain.Common;

namespace Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public ICollection<OrganizationMember> Members { get; set; } = new List<OrganizationMember>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
