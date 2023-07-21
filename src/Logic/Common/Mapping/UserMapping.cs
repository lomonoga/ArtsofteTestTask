using Data.Domain.Models;
using Logic.Common.DTO.Responses;
using Mapster;

namespace Logic.Common.Mapping;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>()
            .Map(ur => ur.Fio, u => u.FIO)
            .Compile();
    }
}