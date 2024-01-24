using api.dtos.auth;
using api.models;
using api.services.auth;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<AuthResponse<CreatedUserDto>>> Login([FromBody]LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _authService.Login(loginDto);
        return response.IsSuccess ? Ok(response) : StatusCode(500, response);
        
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse<CreatedUserDto>>> Register([FromBody] RegisterDto registerDto)
    {
        // validate the properties passed in
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _authService.Register(registerDto);
        return response.IsSuccess ? Ok(response) : StatusCode(500, response);
    }
}