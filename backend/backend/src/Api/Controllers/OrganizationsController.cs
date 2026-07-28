using Application.Features.Organization.Commands.CreateOrganization;
using Application.Features.Organization.Queries.GetMembers;
using Application.Features.Organization.Queries.GetMyWorkspaces;
using Application.Features.Organization.Queries.GetWorkspaceOverview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOverview), new { }, result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyWorkspaces()
    {
        var result = await _mediator.Send(new GetMyWorkspacesQuery());
        return Ok(result);
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _mediator.Send(new GetWorkspaceOverviewQuery());
        return Ok(result);
    }

    [HttpGet("members")]
    public async Task<IActionResult> GetMembers()
    {
        var result = await _mediator.Send(new GetMembersQuery());
        return Ok(result);
    }
}
