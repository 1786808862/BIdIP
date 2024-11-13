using BidIP.Components;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BidIP.Data;
using System.Net;
using BidIP.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<BidIPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BidIPContext") ?? throw new InvalidOperationException("Connection string 'BidIPContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 添加 Swagger 生成器服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();  // 注册内存缓存服务

builder.Services.AddMemoryCache();
// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddControllers();
//builder.Services.AddSingleton<TimedTask>();
builder.Services.AddSingleton<CacheService>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "http://192.168.1.13:7118")
    });// 配置 Kestrel 启动监听局域网 IP
builder.WebHost.ConfigureKestrel(options =>
{
    // 监听所有 IP 地址
    options.Listen(System.Net.IPAddress.Any, 7118);
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
    //var dataService = scope.ServiceProvider.GetRequiredService<TimedTask>();
    //await dataService.InitializeCacheAsync();
}
app.Run();
