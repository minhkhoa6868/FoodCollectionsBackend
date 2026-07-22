using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodCollectionsBackend.Application.Common.Models;
namespace FoodCollectionsBackend.Infrastructure.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;

    public Result<LoginResponse> GenerateToken(User user)
    {
        var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secretKey = Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(
                int.Parse(jwtSettings["TokenLifetimeMinutes"]!)),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return Result<LoginResponse>.SuccessResult(new LoginResponse
        {
            Token = handler.WriteToken(token),
            Expires = token.ValidTo,
            Username = user.Username,
        });
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));
    }
}
