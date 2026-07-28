using Application.Features.Organization.Dtos;
using MediatR;

namespace Application.Features.Organization.Queries.GetMembers;

public record GetMembersQuery() : IRequest<List<MemberDto>>;
