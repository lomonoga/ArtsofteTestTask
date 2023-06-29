using System.Reflection;
using System.Text;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Logic;

public static class ConfigureServices
{
    public static void AddLogic(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    "Memento mori, we're all going to die"))
                };
            });
        services.AddAuthorization();
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(assembly));
        services.AddMapster(assembly);
    }
    private static void AddMapster(this IServiceCollection services, Assembly assembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(assembly);
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
        services.AddSingleton(typeAdapterConfig);
    }

}