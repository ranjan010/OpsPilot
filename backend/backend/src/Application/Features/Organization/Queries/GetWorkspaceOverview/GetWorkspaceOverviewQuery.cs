using Application.Features.Organization.Dtos;
using MediatR;

namespace Application.Features.Organization.Queries.GetWorkspaceOverview;

public record GetWorkspaceOverviewQuery : IRequest<OrganizationOverviewDto>;
