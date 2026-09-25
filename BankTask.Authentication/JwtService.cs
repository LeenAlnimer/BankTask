using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankTask.Application.Interfaces.Services;
using BankTask.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BankTask.Authentication;

public class JwtService : IJwtService
{
    private const int TokenExpirationHours = 1;

    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(
        string secretKey,
        string issuer,
        string audience)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException(
                "JWT secret key cannot be empty.",
                nameof(secretKey));
        }

        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                TokenExpirationHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}