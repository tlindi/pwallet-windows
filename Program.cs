using pWallet.Components;
using pWallet.Controllers;
using pWallet.Services;
using System.Collections.Concurrent;
using pWallet.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:3939");

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddHttpClient<INodeService, PhoenixService>();
builder.Services.AddHttpClient<IPaymentService, PhoenixService>();
builder.Services.AddHttpClient<ILnUrlService, PhoenixService>();

builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddScoped<WebSocketService>();
builder.Services.AddScoped<SseController>();
builder.Services.AddScoped<QrService>();

builder.Services.AddSingleton(new ConcurrentDictionary<string, ConcurrentBag<SseController.SseClient>>());

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	//app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();