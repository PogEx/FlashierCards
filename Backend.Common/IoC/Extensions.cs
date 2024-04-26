using Autofac;
using Backend.Common.IoC.Modules;

namespace Backend.Common.IoC;

public static class Extensions
{
    public static void SetupContainer(this ContainerBuilder builder, string[] args)
    {
        builder.RegisterModule(new CommonModule(args));
    }
}