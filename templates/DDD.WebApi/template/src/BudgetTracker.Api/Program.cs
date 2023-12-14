using Asp.Versioning.ApiExplorer;
using BudgetTracker.Api;
using BudgetTracker.Application;
using BudgetTracker.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Configure Services
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


// Configure the HTTP request pipeline.
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
    app.UseExceptionHandler("/error-development");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
