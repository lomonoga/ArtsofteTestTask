using Api.DTO;
using Data;
using Data.Domain.Models;
using Logic.DTO.Responses;
using Logic.Exceptions;
using Logic.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Handlers.Users;

public record SaveUser(UserRegisterRequest UserRegisterRequest) : IRequest<UserResponse>;

public class SaveUserHandler : IRequestHandler<SaveUser, UserResponse>
{
    private readonly ArtsofteDbContext _context;
    private readonly IHashService _hashService;
    
    public SaveUserHandler(ArtsofteDbContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }
    
    public async Task<UserResponse> Handle(SaveUser request, CancellationToken cancellationToken)
    {
        //Mapping User
        var entityUser = request.UserRegisterRequest.Adapt<User>();
        
        //Hashing password 
        entityUser.Password = _hashService.EncryptPassword(entityUser.Password);
        
        //Check for the existence of a user
        var existedUser = await _context.Users.FirstOrDefaultAsync(u => 
            u.Email == entityUser.Email || u.Phone == entityUser.Phone, cancellationToken);
        if (existedUser is not null)
            throw new UserExistsException();
        
        //Adding user
        var savedUser = (await _context.Users.AddAsync(entityUser, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        _context.Entry(entityUser).State = EntityState.Detached;

        return new UserResponse(savedUser.FIO, savedUser.Phone, savedUser.Email);
    }
}