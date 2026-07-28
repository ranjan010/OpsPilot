using Domain.Common;

namespace Domain.Entities;

public class AuditLog : BaseEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid UserId { get; set; }
    public Users User { get; set; } = default!;

    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string DetailsJson { get; set; } = "{}";
}
