using Application.Features.Projects.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _tenantService;

    public CreateProjectCommandHandler(IApplicationDbContext context, ICurrentTenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantService.HasTenant || !_tenantService.TenantId.HasValue)
        {
            throw new InvalidOperationException("Active tenant context is required.");
        }

        var tenantId = _tenantService.TenantId.Value;
        var normalizedKey = request.Key.Trim().ToUpperInvariant();

        var exists = await _context.Projects
            .AnyAsync(p => p.OrganizationId == tenantId && p.Key == normalizedKey, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException($"Project key '{normalizedKey}' is already in use for this workspace.");
        }

        var project = new Project
        {
            OrganizationId = tenantId,
            Name = request.Name.Trim(),
            Key = normalizedKey,
            Description = request.Description.Trim()
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectDto(project.Id, project.Name, project.Key, project.Description, project.IsArchived, project.CreatedAt);
    }
}
