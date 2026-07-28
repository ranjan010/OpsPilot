using Application.Features.Projects.Dtos;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<List<ProjectDto>>;
