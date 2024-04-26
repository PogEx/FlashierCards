using Autofac;
using Backend.Common.Contracts;
using RestApiBackend.Controllers;

namespace RestApiBackend.IoC.Modules;

public class RestModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserController>().As<IEndpointMapper>();
    }
}