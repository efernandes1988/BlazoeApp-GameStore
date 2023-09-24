using GameStore;
using GameStore.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5020") }); // Aqui ao contr�rio debaixo, inst�ncia no momento o HttpClient, apenas existir� uma inst�ncia para toda a aplica��o (AddScoped)
builder.Services.AddScoped<GameClient>(); // Prepara inst�ncia para quando necess�rio disponibilizar na p�gina que precisar dela

await builder.Build().RunAsync();
