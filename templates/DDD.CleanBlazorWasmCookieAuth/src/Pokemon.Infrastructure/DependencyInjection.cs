using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Application.Common.Interfaces.Services;
using Pokemon.Infrastructure.Persistence;
using Pokemon.Infrastructure.Persistence.Repositories;
using Pokemon.Infrastructure.Services;
using Pokemon.Infrastructure.Authentication;
using Pokemon.Application.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pokemon.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pokemon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddAuth(configuration)
            //.AddMsSqlPersistence(configuration);
            .AddPostgreSqlPersistence(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    //public static IServiceCollection AddMsSqlPersistence(
    //    this IServiceCollection services,
    //    IConfiguration configuration)
    //{
    //    string connectionString = configuration.GetConnectionString("DefaultConnection")!;
    //    services.AddDbContext<PokemonDbContext>(options =>
    //        options.UseSqlServer(connectionString));

    //    services.AddScoped<IUserRepository, UserRepository>();
    //    services.AddScoped<IPocketMonsterRepository, PocketMonsterRepository>();

    //    return services;
    //}


    public static IServiceCollection AddPostgreSqlPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPocketMonsterRepository, PocketMonsterRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // adding custom jwt authentication
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        // adding identity cooke authentication
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        return services;
    }
}