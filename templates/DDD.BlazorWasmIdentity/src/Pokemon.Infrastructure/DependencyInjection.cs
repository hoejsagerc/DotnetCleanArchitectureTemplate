using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Infrastructure.Identity;
using Pokemon.Infrastructure.Persistence;

namespace Pokemon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddPostgreSqlPersistence(configuration)
            .AddIdentity();

        return services;
    }


    public static IServiceCollection AddPostgreSqlPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        // services.AddScoped<IPocketMonsterRepository, PocketMonsterRepository>();
        // services.AddScoped<PublishDomainEventsInterceptor>();
        // services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        return services;
    }


    public static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        return services;
    }

}