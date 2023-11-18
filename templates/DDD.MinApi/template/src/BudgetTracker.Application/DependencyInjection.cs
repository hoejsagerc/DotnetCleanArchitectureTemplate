using System.Reflection;
using BudgetTracker.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetTracker.Application;

public static class DependencyInjection
{
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddHttpContextAccessor();

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehaviorPipeline<,>));
        
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviorPipeline<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}