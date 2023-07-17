using System.Security.Claims;
using Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Logic.Services;

public class SecurityService : ISecurityService
{
    private readonly IHttpContextAccessor _accessor;

    public SecurityService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    
    /// <summary>
    /// Getting current user in system  
    /// </summary>
    /// <returns>User claims</returns>
    public ClaimsPrincipal? GetCurrentUser()
    {
        return _accessor.HttpContext?.User;
    }
}