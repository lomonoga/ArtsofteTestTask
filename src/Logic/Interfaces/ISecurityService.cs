using System.Security.Claims;

namespace Logic.Interfaces;

public interface ISecurityService
{
    public ClaimsPrincipal? GetCurrentUser();
}