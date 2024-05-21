using System.Data;
using Autofac;
using Backend.RestApi.Database;
using MySql.Data.MySqlClient;

namespace Backend.RestApi.IoC.Modules;

public class DbModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FlashiercardsContext>().AsSelf();
    }
}