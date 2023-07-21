using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Domain.Models;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Services;

public class JwtTokenManager : ITokenManager
{
    private readonly IConfiguration _configuration;
    public JwtTokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Generating a token for the user
    /// </summary>
    /// <param name="user">Existing user</param>
    /// <returns>JWT-token that is valid for 2 hours</returns>
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FIO),
            // new(ClaimTypes.MobilePhone, user.Phone),
            new(ClaimTypes.Email, user.Email)
        };
        
        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]!));
        
        var token = new JwtSecurityToken(
            "Artsofte",
            null,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.ToUniversalTime().Add(TimeSpan.FromHours(2)));
        
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}