using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Domain.Models;
using Logic.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Logic.SecurityServices;

public class JwtTokenManager : ITokenManager
{
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FIO),
            new(ClaimTypes.MobilePhone, user.Phone)
        };
        
        const string key = "Memento mori, live and enjoy every moment";
        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        
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