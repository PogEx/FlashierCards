using System.Data.SqlClient;
using Autofac;
using SqlKata.Compilers;

namespace Backend.RestApi.IoC.Modules;

public class DbModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SqlServerCompiler>().As<Compiler>();
        builder.RegisterInstance(new SqlConnection());
    }
}