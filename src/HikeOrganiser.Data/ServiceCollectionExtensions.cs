using HikeOrganiser.Data.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HikeOrganiser.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(opt =>
        {
            string? connectionString = configuration.GetConnectionString("DatabaseConnectionString");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                opt.UseInMemoryDatabase(nameof(Context));
            }
            
            opt.UseSqlServer(connectionString, sqlOpt => sqlOpt.EnableRetryOnFailure());
        });
        
        return services;
    }
}