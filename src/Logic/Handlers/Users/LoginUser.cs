using Data;
using Data.Domain.Models;
using Logic.Common.DTO.Requests;
using Logic.Exceptions;
using Logic.Exceptions.User;
using Logic.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Handlers.Users;

public record LoginUser(UserLoginRequest LoginRequest) : IRequest<User>;

public class LoginUserHandler : IRequestHandler<LoginUser, User>
{
    private ArtsofteDbContext _context;
    private IHashService _hashService;
    
    public LoginUserHandler(ArtsofteDbContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }
    
    public async Task<User> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        // Checking the existence of the user and the correctness of his login and password
        if (request.LoginRequest.Phone is null || request.LoginRequest.Password is null)
            throw new UserDoesNotExistException();
        var hashedPassword = _hashService.EncryptPassword(request.LoginRequest.Password);
        var user = await _context.Users.FirstOrDefaultAsync(u => request.LoginRequest.Phone == u.Phone 
                                                         && hashedPassword == u.Password, cancellationToken);
        if (user is null) throw new UserDoesNotExistException();
        
        // Changing time last login and Activating session
        user.LastLogin = DateTime.UtcNow.ToUniversalTime();
        user.ActiveSession = true;
        await _context.SaveChangesAsync(cancellationToken);
        _context.Entry(user).State = EntityState.Detached;

        return user;
    }
}