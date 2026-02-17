using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SolarWatch.Api.Services;

public class JwtService : IJwtService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _keystring;
    private readonly int _expiresInMinutes;

    public JwtService(IConfiguration config)
    {
        _issuer = config["Jwt:ValidIssuer"] 
                  ?? throw new ArgumentNullException("Jwt:ValidIssuer not configured.");

        _audience = config["Jwt:ValidAudience"] 
                    ?? throw new ArgumentNullException("Jwt:ValidAudience not configured.");

        _keystring = config["Jwt:SigningKey"] 
                     ?? throw new ArgumentNullException("Jwt:SigningKey not configured.");

        _expiresInMinutes = int.Parse(
            config["Jwt:ExpiresInMinutes"] 
            ?? throw new ArgumentNullException("Jwt:ExpiresInMinutes not configured.")
        );
    }

    public string GenerateToken(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_keystring)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}