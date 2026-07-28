namespace Api.src.Application.Features.User.Commands.RegisterUser;

using Api.src.Domain.Interfaces;
using global::Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

// Add the using statement for wherever you define IPasswordHasher, e.g.:
// using Api.src.Application.Interfaces; 

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegistrationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<Users> _passwordHasher;

    // Injected an IPasswordHasher interface for real-world hashing
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<Users> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegistrationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var isUnique = await _userRepository.IsEmailUniqueAsync(request.Email);
        if (!isUnique)
        {
            return new RegistrationResult { Success = false, Message = "Email already in use." };
        }

        // Note: Kept as 'Users' based on your snippet, but singular 'User' is typical for entities
        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        // Use the injected service to hash the password securely
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new RegistrationResult { Success = true, UserId = user.Id };
    }
}