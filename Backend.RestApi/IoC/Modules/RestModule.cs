using Autofac;

namespace RestApiBackend.IoC.Modules;

public class RestModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new AuthModule());
    }
}