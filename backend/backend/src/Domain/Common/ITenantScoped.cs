namespace Domain.Common;

public interface ITenantScoped
{
    public Guid OrganizationId { get; set; }
}
