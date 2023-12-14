using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Asp.Versioning;
using BudgetTracker.Api.Common.Http;
using BudgetTracker.Api.Common.Mappings;
using BudgetTracker.Api.Controllers.OpenApi;
using ErrorOr;

namespace BudgetTracker.Api;


public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        services.ConfigureApiVersioning();
        services.ConfigureSwagger();
        services.AddControllers();
        services.ConfigureProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }

    private static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = (context) =>
            {
                if (context.HttpContext?.Items[HttpContextItemKeys.Errors] is List<Error> errors)
                {
                    context.ProblemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
                }
            }
        );
        return services;
    }

    private static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        return services;
    }
    
    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
            config.UseOneOfForPolymorphism();
            config.SelectDiscriminatorNameUsing(baseType => "TypeName");
            config.SelectDiscriminatorValueUsing(subType => subType.Name);

            var executingAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = executingAssembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                if (referencedAssembly.Name is not null && referencedAssembly.Name.StartsWith("KiboShelters.Contracts"))
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{referencedAssembly.Name}.xml");
                    config.IncludeXmlComments(xmlPath);
                }
            }
            config.SupportNonNullableReferenceTypes();
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();
        return services;
    }
}