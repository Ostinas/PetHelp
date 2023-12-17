using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PetHelp.FrontendWasm;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<TokenAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthStateProvider>());
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7253") });

builder.Services.AddMudServices();

await builder.Build().RunAsync();
