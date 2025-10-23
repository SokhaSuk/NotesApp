using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Services;
using System.Security.Claims;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.AuthenticateAsync(request);
            if (result == null)
            {
                return Unauthorized(new ErrorResponse { Message = "Invalid username or password" });
            }

            return Ok(new ApiResponse<AuthResponse> { Data = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Login), new ApiResponse<AuthResponse> { Data = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred during registration" });
        }
    }

    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            // User info is available in the JWT token claims
            return Ok(new ApiResponse<User> { Data = new User
            {
                Id = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value),
                Username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)!.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)!.Value
            }});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred retrieving user information" });
        }
    }
}
