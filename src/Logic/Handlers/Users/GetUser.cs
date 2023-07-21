using System.Security.Claims;
using Data;
using Logic.Common.DTO.Responses;
using Logic.Exceptions.User;
using Logic.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Handlers.Users;

public record GetUser : IRequest<UserResponse>;

public class GetUserHandler : IRequestHandler<GetUser, UserResponse>
{
    private readonly ArtsofteDbContext _context;
    private readonly ISecurityService _securityService;

    public GetUserHandler(ArtsofteDbContext context, ISecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    public async Task<UserResponse> Handle(GetUser request, CancellationToken cancellationToken)
    {
        var emailUser = _securityService.GetCurrentUser().FindFirstValue(ClaimTypes.Email);
        if (emailUser is null) 
            throw new GetCurrentUserException();
        
        var user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => emailUser == u.Email, cancellationToken);
        if (user is null) 
            throw new UserLogoutException();
        var userAdapt = user.Adapt<UserResponse>();
        return userAdapt;
    }
}