using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthController(UserManager<IdentityUser> userManager, IJwtService jwtService, IConfiguration config)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _config = config;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] UserDto dto)
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "User");
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        var expiresIn = int.Parse(_config["Jwt:ExpiresInMinutes"] ?? throw new ArgumentNullException("Jwt:ExpiresInMinutes not configured"));
        var response = new AuthResponseDto(token, expiresIn);

        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Invalid email or password!");
        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return Unauthorized("Invalid email or password!");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        var expiresIn = int.Parse(
            _config["Jwt:ExpiresInMinutes"]
            ?? throw new InvalidOperationException("Jwt:ExpiresInMinutes not configured")
        );
        var response = new AuthResponseDto(token, expiresIn);

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("make-admin/{userId}")]
    public async Task<IActionResult> MakeAdmin(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"User with id {userId} not found.");

            var isAlreadyAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAlreadyAdmin)
                return BadRequest("User is already an admin.");

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
                return BadRequest($"Failed to make user admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return Ok(new { message = $"User {user.Email} is now an admin." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            message = "PONG!",
            user = User.Identity?.Name
        });
    }

}