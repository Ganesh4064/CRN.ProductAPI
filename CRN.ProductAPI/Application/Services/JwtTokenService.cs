using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Domain.Entities;
using CRN.ProductAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRN.ProductAPI.Application.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public JwtTokenService(
        IConfiguration configuration,
        ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<AuthResponseDto> GenerateTokensAsync(
        string username)
    {
        var accessToken = GenerateAccessToken(username);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Username = username,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>(
                    "Jwt:RefreshTokenDays")),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync();

        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>(
                "Jwt:AccessTokenMinutes"));

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt
        };
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(
        string refreshToken)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                !x.IsRevoked);

        if (storedToken == null ||
            storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        storedToken.IsRevoked = true;

        var newAccessToken =
            GenerateAccessToken(storedToken.Username);

        var newRefreshToken =
            GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            Username = storedToken.Username,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>(
                    "Jwt:RefreshTokenDays")),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(newRefreshTokenEntity);

        await _context.SaveChangesAsync();

        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>(
                "Jwt:AccessTokenMinutes"));

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt =
                newRefreshTokenEntity.ExpiresAt
        };
    }

    private string GenerateAccessToken(string username)
    {
        var key = _configuration["Jwt:Key"]
                  ?? throw new InvalidOperationException(
                      "JWT Key is not configured.");

        var issuer = _configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException(
                         "JWT Issuer is not configured.");

        var audience = _configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException(
                           "JWT Audience is not configured.");

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                username),

            new Claim(
                JwtRegisteredClaimNames.UniqueName,
                username),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>(
                    "Jwt:AccessTokenMinutes")),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }
}