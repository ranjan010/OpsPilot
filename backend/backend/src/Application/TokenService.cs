using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: GetIssuer(),
            audience: GetAudience(),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(GetAccessExpirationMinutes()),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        return !string.IsNullOrWhiteSpace(refreshToken);
    }

    private string GetSecretKey() => _configuration["JwtSettings:SecretKey"] ?? "default-secret";
    private string GetIssuer() => _configuration["JwtSettings:Issuer"] ?? "https://localhost:5001";
    private string GetAudience() => _configuration["JwtSettings:Audience"] ?? "https://localhost:5001";
    private int GetAccessExpirationMinutes() => int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15");
}
