using GameStore;
using GameStore.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5020") }); // Aqui ao contrário debaixo, instância no momento o HttpClient, apenas existirá uma instância para toda a aplicação (AddScoped)
builder.Services.AddScoped<GameClient>(); // Prepara instância para quando necessário disponibilizar na página que precisar dela

await builder.Build().RunAsync();
