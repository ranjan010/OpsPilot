using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_tokenService.ValidateRefreshToken(request.RefreshToken))
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var accessToken = _tokenService.GenerateAccessToken("demo-user", "demo@ai.dev");
        var refreshToken = _tokenService.GenerateRefreshToken();
        return Task.FromResult(new RefreshTokenResult(accessToken, refreshToken));
    }
}
