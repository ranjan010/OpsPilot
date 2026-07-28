using Domain.Enums;

namespace Application.Features.Organization.Dtos;

public record OrganizationDto(
    Guid Id,
    string Name,
    string Slug,
    TenantRole Role,
    DateTime JoinedAt
);

public record OrganizationOverviewDto(
    Guid Id,
    string Name,
    string Slug,
    TenantRole UserRole,
    int TotalMembers,
    int TotalProjects,
    int TotalOpenTickets,
    DateTime CreatedAt
);
