using BidIP.Components;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BidIP.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<BidIPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BidIPContext") ?? throw new InvalidOperationException("Connection string 'BidIPContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMemoryCache();
// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddControllers();
builder.Services.AddSingleton<TimedTask>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7118")
    });
var app = builder.Build();

app.MapControllers();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
// 自动更新数据库
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BidIPContext>();
    context.Database.Migrate();  // 自动迁移
    var dataService = scope.ServiceProvider.GetRequiredService<TimedTask>();
    //await dataService.InitializeCacheAsync();
}
app.Run();
