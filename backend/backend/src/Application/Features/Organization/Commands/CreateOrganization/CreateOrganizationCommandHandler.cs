using Application.Features.Organization.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Organization.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrganizationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<OrganizationDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = _currentUserService.UserId.Value;

        var existingSlug = await _context.Organizations
            .AnyAsync(o => o.Slug == request.Slug.ToLower(), cancellationToken);

        if (existingSlug)
        {
            throw new InvalidOperationException($"Workspace slug '{request.Slug}' is already taken.");
        }

        var organization = new Domain.Entities.Organization
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug.ToLower(),
            CreatedAt = DateTime.UtcNow
        };

        var member = new OrganizationMember
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            UserId = userId,
            Role = TenantRole.Admin,
            JoinedAt = DateTime.UtcNow
        };

        _context.Organizations.Add(organization);
        _context.OrganizationMembers.Add(member);

        await _context.SaveChangesAsync(cancellationToken);

        return new OrganizationDto(
            organization.Id,
            organization.Name,
            organization.Slug,
            TenantRole.Admin,
            member.JoinedAt
        );
    }
}
