using Autofac;
using Autofac.Extensions.DependencyInjection;
using RestApiBackend.Contracts;
using RestApiBackend.Controllers;

namespace RestApiBackend.IoC.Modules
{
    public class DefaultModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HtmlController>().As<IEndpointMapper>();

            AddWebApp(builder);
        }

        private static void AddWebApp(ContainerBuilder builder)
        {
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder();

            webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
        
            WebApplication app = webApplicationBuilder.Build();
            
            app.UseRouting();
            app.UseHttpsRedirection();
        
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            builder.RegisterInstance(app)
                .As<IEndpointRouteBuilder>()
                .As<IHost>()
                .As<IApplicationBuilder>()
                .AsSelf();
        }
    }
}