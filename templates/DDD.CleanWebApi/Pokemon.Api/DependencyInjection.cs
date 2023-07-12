using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Pokemon.Api.Common.Errors;
using Pokemon.Api.Common.Mappings;
using Pokemon.Api.Controllers.Options;

namespace Pokemon.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        services.AddMappings();
        services.AddSwaggerConfig();
        services.AddApiVersioning();

        return services;
    }


    public static IServiceCollection AddSwaggerConfig(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            // Configuring Swagger Authentication
            // ----------------------------------
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Configuring Swagger XML Documentation
            // -------------------------------------
            config.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
            config.UseOneOfForPolymorphism();
            config.SelectDiscriminatorNameUsing(baseType => "TypeName");
            config.SelectDiscriminatorValueUsing(subType => subType.Name);

            var executingAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = executingAssembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                if (referencedAssembly.Name is not null && referencedAssembly.Name.StartsWith("Pokemon.Contracts")) // needs to be changed
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{referencedAssembly.Name}.xml");
                    config.IncludeXmlComments(xmlPath);
                }
            }
            config.SupportNonNullableReferenceTypes();
        });

        // Configuring Swagger Options
        // -------------------------------------
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }


    public static IServiceCollection AddApiVersioning(
        this IServiceCollection services)
    {
        // Configuring Api Versioning
        // --------------------------
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}