using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Common.IoC.Modules
{
    public class CommonModule(string[] args): Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AddWebApp(builder);
        }

        private void AddWebApp(ContainerBuilder builder)
        {
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

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