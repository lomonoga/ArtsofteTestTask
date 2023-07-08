using Api.DTO;
using Logic.DTO.Responses;
using MediatR;

namespace Logic.Handlers.Users;

public record SaveUser(UserRequest UserRequest) : IRequest<UserResponse>;

public class SaveUserHandler : IRequestHandler<SaveUser, UserResponse>
{

    public SaveUserHandler(IMediator mediator)
    {

    }
    
    public Task<UserResponse> Handle(SaveUser request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}