using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.AspNetCore.Components.Authorization;
using VolleyballManager.Client;
using VolleyballManager.Client.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔹 HttpClient konfiguráció JWT token-ekkel
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7187/")
    };
    return client;
});

// 🔹 Authentication services
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// 🔹 AuthService regisztráció
builder.Services.AddScoped<AuthService>();

// 🔹 JSON konfiguráció
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true;
    options.Converters.Add(new JsonStringEnumConverter());
});

await builder.Build().RunAsync();