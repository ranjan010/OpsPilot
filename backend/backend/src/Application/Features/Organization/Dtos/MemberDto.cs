using Domain.Enums;

namespace Application.Features.Organization.Dtos;

public record MemberDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    TenantRole Role,
    DateTime JoinedAt
);
