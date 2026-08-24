using CRN.ProductAPI.Application.DTOs;

namespace CRN.ProductAPI.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(
        AuthRequestDto request);

    Task<AuthResponseDto?> RefreshTokenAsync(
        string refreshToken);
}