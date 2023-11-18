using BudgetTracker.Application.Common.Persistence;
using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Application.Common.Services;
using BudgetTracker.Infrastructure.Persistence;
using BudgetTracker.Infrastructure.Persistence.Interceptors;
using BudgetTracker.Infrastructure.Persistence.Repositories;
using BudgetTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);

        services.AddCustomServices();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IDbConnectionFactory>(provider =>
            new DbConnectionFactory(connectionString));

        services.AddScoped<ITrackableRepository, TrackableRepository>();
        
        return services;
    }


    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();
        
        return services;
    }
}