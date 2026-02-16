namespace SolarWatch.Api.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
    public UserInfoDto User { get; set; }

    public AuthResponseDto(string token, int expiresIn, UserInfoDto user)
    {
        Token = token;
        ExpiresIn = expiresIn;
        User = user;
    }
}
