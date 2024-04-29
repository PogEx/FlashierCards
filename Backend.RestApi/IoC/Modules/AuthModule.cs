using Autofac;
using Backend.RestApi.Authentication;
using Backend.RestApi.Contracts.Auth;

namespace Backend.RestApi.IoC.Modules;

public class AuthModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<JwtTokenManager>().As<ITokenManager>().SingleInstance();
        builder.RegisterType<UserHandler>().As<IUserHandler>().SingleInstance();
    }
}