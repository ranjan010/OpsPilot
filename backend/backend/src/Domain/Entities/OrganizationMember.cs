using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class OrganizationMember : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid UserId { get; set; }
    public Users User { get; set; } = default!;

    public TenantRole Role { get; set; } = TenantRole.Agent;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
