using Domain.Enums;

namespace Application.Interfaces;

public interface ICurrentTenantService
{
    Guid? TenantId { get; set; }
    bool HasTenant { get; }
    TenantRole? UserRole { get; set; }
}
