using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankTask.Application.Interfaces.Services;
using BankTask.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BankTask.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtOptions _options;

    public JwtService(JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            throw new ArgumentException(
                "JWT secret key cannot be empty.",
                nameof(options.SecretKey));
        }

        _options = options;
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
            Encoding.UTF8.GetBytes(_options.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                _options.ExpirationHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}