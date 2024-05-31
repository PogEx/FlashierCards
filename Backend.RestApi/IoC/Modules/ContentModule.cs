using Autofac;
using Backend.RestApi.ContentHandlers;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Contracts.DatabaseOperator;

namespace Backend.RestApi.IoC.Modules;

public class ContentModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FolderHandler>().As<IFolderHandler>();
        builder.RegisterType<DeckHandler>()
            .As<IDeckHandler>()
            .As<IDeckHandlerInternal>();
        builder.RegisterType<CardHandler>().As<ICardHandler>();
    }
}