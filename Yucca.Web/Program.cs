using Yucca.Web;
using Yucca.Web.Components;
using Yucca.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var apiEndpoint = builder.Configuration["ApiEndpoint"]
    ?? throw new InvalidOperationException("ApiEndpoint is not set");

builder.Services.AddSingleton<SupplierService>();
builder.Services.AddHttpClient<SupplierService>(c => c.BaseAddress = new Uri(apiEndpoint));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();