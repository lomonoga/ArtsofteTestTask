using Api.DTO;
using Data;
using Data.Domain.Models;
using Logic.DTO.Responses;
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
        var entity = request.UserRegisterRequest.Adapt<User>();
        
        var savedUser = (await _context.Users.AddAsync(entity, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        _context.Entry(entity).State = EntityState.Detached;

        return new UserResponse(savedUser.FIO, savedUser.Phone, savedUser.Email);
    }
}