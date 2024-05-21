using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.RestApi.IoC.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Backend.RestApi;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        webApplicationBuilder.Host.ConfigureContainer(
            (HostBuilderContext context, ContainerBuilder builder) => 
                SetupAutofacContainer(context, builder));

        webApplicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            IConfigurationSection jwtSection = webApplicationBuilder.Configuration.GetSection("Jwt");
            byte[] keyByte = Encoding.ASCII.GetBytes(jwtSection.GetValue<string>("Key") ?? "");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keyByte),
                ValidIssuer = jwtSection.GetValue<string>("Issuer"),
                ValidAudience = jwtSection.GetValue<string>("Audience"),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true
            };
        });
        webApplicationBuilder.Services.AddAuthorization();
        
        webApplicationBuilder.Services
            .AddControllers()
            .AddControllersAsServices();

        webApplicationBuilder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Flashier Cards Api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert JWT with Bearer into field",
                Scheme = "bearer",
                BearerFormat = "JWT",
                Type = SecuritySchemeType.Http 
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    new string[] { } 
                } 
            });
        });
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        
        WebApplication app = webApplicationBuilder.Build();
        
        app.UseRouting();
        app.UseHttpsRedirection();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }

    private static void SetupAutofacContainer(HostBuilderContext _, ContainerBuilder builder)
    {
        builder.RegisterModule(new RestModule());
    }
}