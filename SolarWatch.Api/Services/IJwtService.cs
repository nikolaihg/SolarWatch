using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Api.Services;

public interface IJwtService
{
    string GenerateToken(IdentityUser user, IEnumerable<string> roles);
}