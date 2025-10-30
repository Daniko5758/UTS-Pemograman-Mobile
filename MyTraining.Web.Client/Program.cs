using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyTraining.Shared.Services;
using MyTraining.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the MyTraining.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/"); // sesuaikan port backend
});

await builder.Build().RunAsync();
