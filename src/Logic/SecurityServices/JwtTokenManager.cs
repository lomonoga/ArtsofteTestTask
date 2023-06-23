using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Logic.SecurityServices;

public static class JwtTokenManager
{
    public static string GenerateToken()
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, )
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]!));
        var token = new JwtSecurityToken(
            null,
            null,
            expires: expiresAt,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

}