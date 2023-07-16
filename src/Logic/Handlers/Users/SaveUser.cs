using Api.DTO;
using Data;
using Data.Domain.Models;
using Logic.DTO.Responses;
using Logic.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Handlers.Users;

public record SaveUser(UserRegisterRequest UserRegisterRequest) : IRequest<UserResponse>;

public class SaveUserHandler : IRequestHandler<SaveUser, UserResponse>
{
    private ArtsofteDbContext _context;
    
    public SaveUserHandler(ArtsofteDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserResponse> Handle(SaveUser request, CancellationToken cancellationToken)
    {
        var entityUser = request.UserRegisterRequest.Adapt<User>();
        
        var existedUser = await _context.Users.FirstOrDefaultAsync(u => 
            u.Email == entityUser.Email || u.Phone == entityUser.Phone, cancellationToken);
        if (existedUser is not null)
            throw new UserExistsException();
        
        var savedUser = (await _context.Users.AddAsync(entityUser, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        _context.Entry(entityUser).State = EntityState.Detached;

        return new UserResponse(savedUser.FIO, savedUser.Phone, savedUser.Email);
    }
}