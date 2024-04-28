using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using RestApiBackend.IoC.Modules;

namespace RestApiBackend;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        webApplicationBuilder.Host.ConfigureContainer(
            (HostBuilderContext context, ContainerBuilder builder) => 
                SetupAutofacContainer(context, builder));

        webApplicationBuilder.Services.AddAuthentication();
        webApplicationBuilder.Services.AddAuthorization();
        
        webApplicationBuilder.Services
            .AddControllers()
            .AddControllersAsServices();

        webApplicationBuilder.Services.AddSwaggerGen(c => c.EnableAnnotations());
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        
        WebApplication app = webApplicationBuilder.Build();
        
        app.UseRouting();
        app.UseHttpsRedirection();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHsts();
        }
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }

    private static void SetupAutofacContainer(HostBuilderContext context, ContainerBuilder builder)
    {
        builder.RegisterModule(new RestModule());
    }
}