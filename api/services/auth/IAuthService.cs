using api.dtos.auth;
using api.models;

namespace api.services.auth;

public interface IAuthService
{
    public Task<AuthResponse<CreatedUserDto>> Register(RegisterDto registerDto);
    public Task<AuthResponse<CreatedUserDto>> Login(LoginDto loginDto);
    string GenerateJwtToken(AppUser user);
    

}