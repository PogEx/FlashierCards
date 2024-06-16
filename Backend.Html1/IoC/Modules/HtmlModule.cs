using Autofac;
using Backend.Common.Contracts;
using Backend.Html.Controllers;

namespace Backend.Html.IoC.Modules;

public class HtmlModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HtmlController>()
            .As<IEndpointMapper>()
            .SingleInstance();
    }
}