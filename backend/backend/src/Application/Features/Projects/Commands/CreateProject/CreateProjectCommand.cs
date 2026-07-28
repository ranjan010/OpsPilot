using Application.Features.Projects.Dtos;
using MediatR;

namespace Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string Key, string Description) : IRequest<ProjectDto>;
