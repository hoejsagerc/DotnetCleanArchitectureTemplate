using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pokemon.Client;
using MudBlazor.Services;
using Pokemon.Client.Services.v1;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp =>
    new ApiClientV1(
        new Uri(builder.HostEnvironment.BaseAddress).ToString(),
        new HttpClient()));

builder.Services.AddMudServices();

await builder.Build().RunAsync();