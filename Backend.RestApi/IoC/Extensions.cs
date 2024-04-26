using Autofac;
using RestApiBackend.IoC.Modules;

namespace RestApiBackend.IoC;

public static class Extensions
{
    public static void SetupContainer(this ContainerBuilder builder)
    {
        builder.RegisterModule(new DefaultModule());
    }
}