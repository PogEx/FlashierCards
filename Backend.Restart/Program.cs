using Backend.Database.Database.Configs;
using Backend.Database.Database.Context;
using Backend.Restart.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<FlashiercardsContext>(options =>
{
    options.UseMySql(builder.Configuration.GetSection("ConnectionStrings").Get<DbConfig>()?.Mysqldb,
        ServerVersion.Parse("8.4.0-mysql"));
});

builder.Configuration
    .AddJsonFile(Environment.GetEnvironmentVariable("FLASHIERCARDS_BACKEND_CONFIG_PATH") + "db.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
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

using (FlashiercardsContext context = await app.Services.GetRequiredService<IDbContextFactory<FlashiercardsContext>>().CreateDbContextAsync())
{
    context.Database.EnsureCreated();
}

app.Run();
