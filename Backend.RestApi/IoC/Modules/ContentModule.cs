using Autofac;
using Backend.RestApi.ContentHandlers;
using Backend.RestApi.Contracts.Content;

namespace Backend.RestApi.IoC.Modules;

public class ContentModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FolderHandler>().As<IFolderHandler>();
        builder.RegisterType<DeckHandler>().As<IDeckHandler>();
        builder.RegisterType<CardHandler>().As<ICardHandler>();
    }
}