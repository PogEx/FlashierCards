using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.Html.Components;
using Backend.Html.IoC;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Backend.Html;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        webApplicationBuilder.Host.ConfigureContainer(
            (HostBuilderContext context, ContainerBuilder builder) => 
                SetupAutofacContainer(context, builder));

        webApplicationBuilder.Configuration.AddJsonFile(
            Environment.GetEnvironmentVariable("FLASHIERCARDS_FRONTEND_CONFIG_PATH") + "backend.json", optional: false,
            reloadOnChange: true);
        
        webApplicationBuilder.Services.AddSerilog((services, lc) => 
            lc.ReadFrom.Configuration(webApplicationBuilder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

        webApplicationBuilder.Services.AddBlazoredLocalStorage();
        webApplicationBuilder.Services.AddBlazoredSessionStorage();
        
        // Add services to the container.
        webApplicationBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();

        WebApplication app = webApplicationBuilder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseSerilogRequestLogging();
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();

        app.Run();
    }
    
    private static void SetupAutofacContainer(HostBuilderContext _, ContainerBuilder builder)
    {
        builder.RegisterInstance(Log.Logger).As<ILogger>();
        builder.RegisterModule<HtmlModule>();
    }
}