using Application.Features.Organization.Dtos;
using MediatR;

namespace Application.Features.Organization.Commands.CreateOrganization;

public record CreateOrganizationCommand(string Name, string Slug) : IRequest<OrganizationDto>;
