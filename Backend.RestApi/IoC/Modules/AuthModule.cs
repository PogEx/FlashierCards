using Autofac;
using RestApiBackend.Authentication;
using RestApiBackend.Contracts.Auth;

namespace RestApiBackend.IoC.Modules;

public class AuthModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<JwtTokenManager>().As<ITokenManager>().SingleInstance();
        builder.RegisterType<UserHandler>().As<IUserHandler>().SingleInstance();
    }
}