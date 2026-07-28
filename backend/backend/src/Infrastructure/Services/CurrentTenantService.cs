using Application.Interfaces;
using Domain.Enums;

namespace Infrastructure.Services;

public class CurrentTenantService : ICurrentTenantService
{
    public Guid? TenantId { get; set; }
    public bool HasTenant => TenantId.HasValue && TenantId.Value != Guid.Empty;
    public TenantRole? UserRole { get; set; }
}
