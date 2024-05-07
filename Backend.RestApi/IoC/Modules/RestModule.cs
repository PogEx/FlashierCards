using Autofac;

namespace Backend.RestApi.IoC.Modules;

public class RestModule(IConfiguration configuration): Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new AuthModule());
        builder.RegisterModule(new DbModule(configuration));
    }
}