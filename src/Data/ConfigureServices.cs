using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ConfigureServices
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ArtsofteDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("def_connection"));
        }, ServiceLifetime.Transient);
    }
}