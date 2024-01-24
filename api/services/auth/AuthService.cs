using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.dtos.auth;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace api.services.auth;

public class AuthService: IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly SymmetricSecurityKey _securityKey;

    public AuthService(UserManager<AppUser> userManager, IConfiguration configuration, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
        // create a bytes symmetric key from out token stored in app settings
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
    }
    
    public async Task<AuthResponse<CreatedUserDto>> Register(RegisterDto registerDto)
    {
        var response = new AuthResponse<CreatedUserDto>();
        var appUser = new AppUser()
        {
            UserName = registerDto.Username,
            Email = registerDto.EmailAddress,
        };
        try
        {
            var newUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!newUser.Succeeded)
            {
                foreach (var error in newUser.Errors)
                {
                    response.Errors?.Add(error.Description);
                }

                throw new Exception("New user creation failed");
            }
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
            {
                foreach (var roleResultError in roleResult.Errors)
                {
                    response.Errors.Add(roleResultError.Description);
                }
                throw new Exception("User role creation failed");
            }
            response.IsSuccess = true;
            response.Message = "Successfully registered user";

            response.Data = new CreatedUserDto()
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = GenerateJwtToken(appUser)
            };

        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Errors?.Add(e.Message);
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<AuthResponse<CreatedUserDto>> Login(LoginDto loginDto)
    {
        var response = new AuthResponse<CreatedUserDto>();
        try
        {
            var matchedUser =
                await _userManager.Users.FirstOrDefaultAsync(user =>
                    user.UserName == loginDto.Username.ToLower());

            if (matchedUser is null)
            {
                throw new Exception("There was no matched user");
            }

            var signIn =
                await _signInManager.CheckPasswordSignInAsync(matchedUser, loginDto.Password,
                    false);

            if (!signIn.Succeeded)
            {
                throw new Exception("Password or Email is incorrect");
            }

            response.Data = new CreatedUserDto()
            {
                Email = matchedUser.Email,
                UserName = matchedUser.UserName,
                Token = GenerateJwtToken(matchedUser)
            };
            
            response.IsSuccess = true;
            response.Message = "Successfully logged the user";

        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }


    public string GenerateJwtToken(AppUser user)
    {
        var claims = new List<Claim>();
        
        // add claims here
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.UserName));
        
        // create signing credentials: what encryption 
        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
        var issuer = _configuration["JWT:Issuer"]!;
        var audience = _configuration["JWT:Audience"]!;
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        // init token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // get the token back as a string
        return tokenHandler.WriteToken(token);
    }
}