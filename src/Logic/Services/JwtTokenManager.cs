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
    
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FIO),
            new(ClaimTypes.MobilePhone, user.Phone)
        };
        
        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]!));
        
        var token = new JwtSecurityToken(
            "Artsofte",
            null,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)));
        
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}