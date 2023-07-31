using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Application.Common.Interfaces.Services;
using Pokemon.Infrastructure.Persistence;
using Pokemon.Infrastructure.Persistence.Repositories;
using Pokemon.Infrastructure.Services;
using Pokemon.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pokemon.Infrastructure.Authentication;
using System.Text;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Options;

namespace Pokemon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddPostgreSqlPersistence(configuration)
            .AddAuthentication(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddPostgreSqlPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDefaultIdentity<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IPocketMonsterRepository, PocketMonsterRepository>();
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        return services;
    }


    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configurations)
    {
        // var jwtSettings = configurations.GetSection("JwtSettings").Get<JwtSettings>();
        var jwtSettings = new JwtSettings();
        configurations.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings!.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

}