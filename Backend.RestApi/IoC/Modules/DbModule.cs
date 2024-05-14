using System.Data;
using Autofac;
using Backend.RestApi.TypeHandlers;
using Dapper;
using MySql.Data.MySqlClient;

namespace Backend.RestApi.IoC.Modules;

public class DbModule(IConfiguration configuration) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(new MySqlConnection(configuration.GetConnectionString("mysqldb")))
            .As<IDbConnection>().SingleInstance();
        
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));
    }
}