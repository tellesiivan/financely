namespace api.dtos.auth;

public class CreatedUserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public required string Token { get; set; }
}