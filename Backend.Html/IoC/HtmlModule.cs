using Autofac;
using Backend.Html.Services;
using Backend.Html.Services.Contracts;

namespace Backend.Html.IoC;

public class HtmlModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AuthorizationProvider>().As<IAuthorizationHandler>();
        builder.RegisterType<ThemeService>().As<IThemeService>().SingleInstance();
        builder.RegisterType<RestClientProvider>().As<IRestClientProvider>().SingleInstance();
        builder.RegisterType<Cookie>().As<ICookie>();
    }
}