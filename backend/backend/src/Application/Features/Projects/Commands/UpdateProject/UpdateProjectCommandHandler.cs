using Application.Features.Projects.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _currentTenantService;

    public UpdateProjectCommandHandler(IApplicationDbContext context, ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenantService.TenantId;
        if (tenantId == Guid.Empty)
        {
            throw new InvalidOperationException("Tenant ID is not set.");
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.OrganizationId == tenantId && !p.IsArchived, cancellationToken);

        if (project is null)
        {
            throw new KeyNotFoundException($"Project '{request.Id}' was not found in the active workspace.");
        }

        project.Name = request.Name;
        project.Description = request.Description;
        project.IsArchived = request.IsArchived;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectDto(project.Id, project.Name, project.Key, project.Description, project.IsArchived, project.CreatedAt);
    }
}