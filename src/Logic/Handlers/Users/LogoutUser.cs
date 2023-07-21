using System.Security.Claims;
using Data;
using Logic.Exceptions.User;
using Logic.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Handlers.Users;

public record LogoutUser : IRequest<Task>;

public class LogoutUserHandler : IRequestHandler<LogoutUser, Task>
{
    private readonly ArtsofteDbContext _context;
    private readonly ISecurityService _securityService;

    public LogoutUserHandler(ArtsofteDbContext context, ISecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    public async Task<Task> Handle(LogoutUser request, CancellationToken cancellationToken)
    {
        var emailUser = _securityService.GetCurrentUser().FindFirstValue(ClaimTypes.Email);
        if (emailUser is null) 
            throw new UserLogoutException();
        
        var user = await _context.Users.FirstOrDefaultAsync(u => emailUser == u.Email, cancellationToken);
        if (user is null) 
            throw new UserLogoutException();
        
        user.ActiveSession = false;
        await _context.SaveChangesAsync(cancellationToken);
        _context.Entry(user).State = EntityState.Detached;
        
        return Task.CompletedTask;
    }
}