using Data.Domain.Models;

namespace Logic.Interfaces;

public interface ITokenManager
{
    public string GenerateToken(User user);
}