using HikeOrganiser.Data.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HikeOrganiser.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HikeOrganiserContext>(opt =>
        {
            string? connectionString = configuration.GetConnectionString("HikeOrganiser");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                opt.UseInMemoryDatabase(nameof(HikeOrganiserContext));
            }
            else
            {
                opt.UseSqlServer(connectionString, o =>
                {
                    o.EnableRetryOnFailure();
                });
            }

            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
        });
        
        
        
        return services;
    }
}