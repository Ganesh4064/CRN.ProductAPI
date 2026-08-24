using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;

namespace CRN.ProductAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto?> LoginAsync(
        AuthRequestDto request)
    {
        // Temporary assessment user.
        // In a production application, credentials should
        // be validated against a users table/identity provider.

        if (request.Username != "ganesh" ||
            request.Password != "Ganesh@2001")
        {
            return null;
        }

        return await _jwtTokenService.GenerateTokensAsync(
            request.Username);
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(
        string refreshToken)
    {
        return await _jwtTokenService.RefreshTokenAsync(
            refreshToken);
    }
}