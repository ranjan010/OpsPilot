using Application.Features.Organization.Dtos;
using MediatR;

namespace Application.Features.Organization.Queries.GetMyWorkspaces;

public record GetMyWorkspacesQuery : IRequest<List<OrganizationDto>>;
