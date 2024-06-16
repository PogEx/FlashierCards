using Autofac;
using Backend.Database.IoC;

namespace Backend.RestApi.IoC.Modules;

public class RestModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new AuthModule());
        builder.RegisterModule(new DbModule());
        builder.RegisterModule(new ContentModule());
    }
}