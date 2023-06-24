using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Logic.SecurityServices;

public static class JwtTokenManager
{
    public static string GenerateToken(string phone)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, phone)
        };
        
        const string key = "Memento mori, we're all going to die";
        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        
        var token = new JwtSecurityToken(
            null,
            null,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)));
        
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

}