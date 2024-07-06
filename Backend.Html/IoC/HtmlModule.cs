using Autofac;
using Backend.Html.Services;
using Backend.Html.Services.Contracts;

namespace Backend.Html.IoC;

public class HtmlModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AuthorizationProvider>().As<IAuthorizationHandler>().SingleInstance();
        builder.RegisterType<RestClientProvider>().As<IRestClientProvider>();
    }
}