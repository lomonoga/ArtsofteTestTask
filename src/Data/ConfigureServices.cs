using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ConfigureServices
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddDbContext<>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("pos_connection")!,
                opts => opts.MigrationsAssembly(assembly.GetName().Name));
        });
        services.AddScoped<>();

    }
}