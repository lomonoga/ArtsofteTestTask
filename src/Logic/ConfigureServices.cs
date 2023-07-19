using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Data;
using Logic.Interfaces;
using Logic.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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
        
        services.AddHttpContextAccessor();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddSingleton<ITokenManager, JwtTokenManager>();
        services.AddSingleton<IHashService, HashService>();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "Artsofte",
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration["JWT:SecretKey"]!))
                };
                // Adding verification of the jwt token for login and logout by receiving a user from jwt by Email
                // and verifying this user to log in or log out 
                // When logging in, ActiveSession = true
                // When logging out, ActiveSession = false
                opt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var jwtToken = context.HttpContext.Request.Headers["Authorization"]
                            .FirstOrDefault()?
                            .Split(" ")
                            .Last();
                        
                        if (jwtToken is null) context.Fail("Token revoked");

                        var userEmail = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken)
                            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        
                        if (userEmail is null) context.Fail("Token revoked");
                        
                        using (var dbContext = context.HttpContext
                                   .RequestServices.GetRequiredService<ArtsofteDbContext>())
                        {
                            if (dbContext.Users.AnyAsync(user => 
                                    user.Email == userEmail
                                    && !user.ActiveSession).Result
                                ) 
                                context.Fail("Token revoked");
                        }
                        
                        return Task.CompletedTask;
                    }
                };
                // opt.SaveToken = true;
                // opt.RequireHttpsMetadata = true;
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
    
    // Adding jwt Tokens to Swagger Requests 
    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ArtSofte"
                });
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
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}