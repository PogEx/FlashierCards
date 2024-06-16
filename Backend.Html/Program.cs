using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.Html.Components;
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
        
        webApplicationBuilder.Services.AddSerilog((services, lc) => 
            lc.ReadFrom.Configuration(webApplicationBuilder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
        
        // Add services to the container.
        webApplicationBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

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
            .AddInteractiveServerRenderMode();

        app.Run();
    }
    
    private static void SetupAutofacContainer(HostBuilderContext _, ContainerBuilder builder)
    {
        builder.RegisterInstance(Log.Logger).As<ILogger>();
    }
}