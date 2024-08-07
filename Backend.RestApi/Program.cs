using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.RestApi.IoC.Modules;
using Backend.RestApi.Logging;
using FluentResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using ILogger = Serilog.ILogger;

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

        webApplicationBuilder.Configuration.AddJsonFile(Environment.GetEnvironmentVariable("FLASHIERCARDS_BACKEND_CONFIG_PATH") + "db.json", optional: false, reloadOnChange: true);
        webApplicationBuilder.Configuration.AddJsonFile(Environment.GetEnvironmentVariable("FLASHIERCARDS_BACKEND_CONFIG_PATH") + "jwt.json", optional: false, reloadOnChange: true);

        webApplicationBuilder.Services.AddSerilog((services, lc) => 
            lc.ReadFrom.Configuration(webApplicationBuilder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
        
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
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
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
        InitResultLogger(Log.Logger);
        
        app.UseRouting();
        app.UseHttpsRedirection();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSerilogRequestLogging();
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        
        app.Run();
    }

    private static void SetupAutofacContainer(HostBuilderContext _, ContainerBuilder builder)
    {
        builder.RegisterModule(new RestModule());
        builder.RegisterInstance(Log.Logger).As<ILogger>();
    }

    private static void InitResultLogger(ILogger logger)
    {
        Result.Setup(cfg => cfg.Logger = new ResultLogger(logger));
    }
}