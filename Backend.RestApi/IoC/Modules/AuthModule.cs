using Autofac;
using Backend.RestApi.ContentHandlers;
using Backend.RestApi.Contracts.Auth;
using Backend.RestApi.Contracts.Content;

namespace Backend.RestApi.IoC.Modules;

public class AuthModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<JwtTokenHandler>().As<ITokenManager>().SingleInstance();
        builder.RegisterType<UserHandler>().As<IUserHandler>().SingleInstance();
    }
}