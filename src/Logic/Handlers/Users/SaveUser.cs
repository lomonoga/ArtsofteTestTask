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
        await _context.Users.AddAsync(request.UserRegisterRequest.Adapt<User>(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new UserResponse("ddd", "ddd", "dsd");
    }
}