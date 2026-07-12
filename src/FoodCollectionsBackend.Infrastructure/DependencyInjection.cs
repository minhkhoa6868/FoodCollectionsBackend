using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FoodCollectionsBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace FoodCollectionsBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
