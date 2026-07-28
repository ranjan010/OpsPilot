namespace Api.src.Application.Features.User.Commands.RegisterUser;

using MediatR;
using System;

public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest<RegistrationResult>;

public record RegistrationResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public Guid? UserId { get; init; }
}