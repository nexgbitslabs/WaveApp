using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WaveApp.Core.DTOs;
using WaveApp.Core.Interfaces;

namespace WaveApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }


    // =========================================================
    // REGISTER
    // POST: /api/auth/register
    // =========================================================

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterReadDto>> Register(
        [FromBody] RegisterDto dto)
    {
        try
        {
            var result =
                await _authService.RegisterAsync(dto);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // =========================================================
    // LOGIN
    // POST: /api/auth/login
    // =========================================================

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] AuthLoginDto dto)
    {
        var result =
            await _authService.LoginAsync(
                dto.Username,
                dto.Password);

        if (result == null)
        {
            return Unauthorized(new
            {
                message =
                    "Invalid username or password."
            });
        }

        return Ok(result);
    }


    // =========================================================
    // REFRESH TOKEN
    // POST: /api/auth/refresh
    // =========================================================

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        [FromBody] RefreshTokenDto dto)
    {
        var result =
            await _authService.RefreshAsync(
                dto.RefreshToken);

        if (result == null)
        {
            return Unauthorized(new
            {
                message =
                    "Invalid or expired refresh token."
            });
        }

        return Ok(result);
    }


    // =========================================================
    // LOGOUT
    // POST: /api/auth/logout
    // =========================================================

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutDto dto)
    {
        await _authService.LogoutAsync(
            dto.RefreshToken);

        return NoContent();
    }


    // =========================================================
    // CURRENT PROFILE
    // GET: /api/auth/profile
    // =========================================================

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ProfileReadDto>> GetProfile()
    {
        var username =
            User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized();
        }

        var profile =
            await _authService.GetProfileAsync(
                username);

        if (profile == null)
        {
            return NotFound(new
            {
                message =
                    "Profile was not found."
            });
        }

        return Ok(profile);
    }
}