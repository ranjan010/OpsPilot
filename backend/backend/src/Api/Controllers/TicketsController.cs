using Application.Features.Tickets.Commands.CreateTicket;
using Application.Features.Tickets.Commands.UpdateTicketStatus;
using Application.Features.Tickets.Queries.GetTickets;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetTicketsQuery());
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest body)
    {
        var command = new UpdateTicketStatusCommand(id, body.Status);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

public record UpdateStatusRequest(TicketStatus Status);
