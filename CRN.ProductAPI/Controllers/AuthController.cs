using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN.ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login(
        AuthRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password."
            });
        }

        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest(new
            {
                message = "Refresh token is required."
            });
        }

        var response =
            await _authService.RefreshTokenAsync(refreshToken);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid or expired refresh token."
            });
        }

        return Ok(response);
    }
}