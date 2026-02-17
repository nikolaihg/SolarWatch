namespace SolarWatch.Api.DTOs;

public class UserInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }

    public UserInfoDto(string id, string email)
    {
        Id = id;
        Email = email;
    }
}
