using Autofac;
using Backend.Database.Database.Context;

namespace Backend.Database.IoC;

public class DbModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FlashiercardsContext>().AsSelf().InstancePerLifetimeScope();
    }
}