//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeFirstJWT1.Models;

namespace CodeFirstJWT1.Services;


public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // Retrieve values from configuration and check if they are null
        var keyString = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyString))
        {
            throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured properly.");
        }

        var issuer = _configuration["Jwt:Issuer"];
        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer is not configured properly.");
        }

        var audience = _configuration["Jwt:Audience"];
        if (string.IsNullOrEmpty(audience))
        {
            throw new ArgumentNullException("Jwt:Audience", "JWT Audience is not configured properly.");
        }

        // Proceed to generate the key and token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
