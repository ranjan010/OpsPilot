using Application.Features.Projects.Dtos;
using MediatR;
using System;

namespace Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string Description, bool IsArchived) : IRequest<ProjectDto>;
