using Autofac;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.ContentHandlers;
using Backend.RestApi.Contracts.Content;

namespace Backend.RestApi.IoC.Modules;

public class ContentModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FolderHandler>().As<IFolderHandler>().As<IFolderRootHandler>();
        builder.RegisterType<DeckHandler>()
            .As<IDeckHandler>()
            .Keyed<IShareable<string>>(typeof(Deck));
        builder.RegisterType<CardHandler>().As<ICardHandler>();
    }
}