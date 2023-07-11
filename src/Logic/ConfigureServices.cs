using System.Reflection;
using System.Text;
using Logic.Interfaces;
using Logic.SecurityServices;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Logic;

public static class ConfigureServices
{
    public static void AddLogic(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddSingleton<IHashService, HashService>();
        services.AddSingleton<ITokenManager, JwtTokenManager>();
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
        services.AddSwagger();
    }
    private static void AddMapster(this IServiceCollection services, Assembly assembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(assembly);
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
        services.AddSingleton(typeAdapterConfig);
    }
    
    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer ‘token‘"
                });
        });
    }

}