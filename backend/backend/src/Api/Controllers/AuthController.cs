using Api.src.Application.Features.User.Commands.RegisterUser;
using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new { message = "Authenticated user" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await _mediator.Send(new RegisterUserCommand(request.Email, request.Password, request.FirstName, request.LastName));
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}

public record LoginRequest(string Email, string Password);
public record RegisterUserRequest(string Email, string Password, string FirstName, string LastName);