using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BudgetTracker.Api;
using BudgetTracker.Api.Endpoints.Trackable.v1;
using BudgetTracker.Application;
using BudgetTracker.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.ApiVersion.ToString());
        }
    });
    app.UseExceptionHandler();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(exceptionHandlerApp
        => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));
}

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.MapTrackableEndpoints();
app.Run();