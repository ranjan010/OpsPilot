namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string email);
    string GenerateRefreshToken();
    bool ValidateRefreshToken(string refreshToken);
}
