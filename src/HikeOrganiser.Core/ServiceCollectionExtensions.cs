using System.Reflection;
using HikeOrganiser.Core.Services;
using HikeOrganiser.Data;
using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace HikeOrganiser.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly asm = typeof(ServiceCollectionExtensions).Assembly;

        services.AddHealthChecks()
            .AddDbContextCheck<HikeOrganiserContext>();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<HikeOrganiserContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization();
        
        services.AddAutoMapper(asm);
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(asm);
        });

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IEmailSender<User>, IdentityEmailSender>();
        
        services.AddCoreContext(configuration);

        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
        });
        
        return services;
    }
}