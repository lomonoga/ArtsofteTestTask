// using Data;
// using MediatR;
//
// namespace Logic.Handlers.Users;
//
// public record GetUser() : IRequest<UserResponse>;
//
// public class GetUserHandler : IRequestHandler<SaveUser, UserResponse>
// {
//     private ArtsofteDbContext _context;
//     
//     public GetUserHandler(ArtsofteDbContext context)
//     {
//         _context = context;
//     }
//     
//     public async Task<UserResponse> Handle(SaveUser request, CancellationToken cancellationToken)
//     {
//         return;
//     }
// }