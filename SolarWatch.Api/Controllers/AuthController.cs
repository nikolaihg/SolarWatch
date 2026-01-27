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
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token, expiresIn = int.Parse(_config["JWT_EXPIRES_IN_MINUTES"] ?? throw new ArgumentNullException("JWT_EXPIRES_IN_MINUTES not configured")),
            user = new
            {
                user.Id, user.Email
            }
        });
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Invalid email or password!");
        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return Unauthorized("Invalid email or password!");
        var token = _jwtService.GenerateToken(user);
        
        return Ok(new
        {
            token,
            expiresIn = int.Parse(
                _config["JWT_EXPIRES_IN_MINUTES"]
                ?? throw new InvalidOperationException("JWT_EXPIRES_IN_MINUTES not configured")
            ),
            user = new { user.Id, user.Email }
        });
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