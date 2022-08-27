using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            opts => opts.UseNpgsql(
                configuration.GetConnectionString("db"), 
                o =>
                {
                    o.UseNetTopologySuite();
                    o.MigrationsAssembly("WebApi");
                })
        );
        return services;
    }
}