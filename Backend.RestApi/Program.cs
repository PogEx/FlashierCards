using Autofac;
using Backend.Common.Contracts;
using Backend.Common.IoC;
using RestApiBackend.IoC.Modules;

namespace RestApiBackend;

public class Program
{
    public static void Main(string[] args)
    {
        ContainerBuilder containerBuilder = new ();
        
        containerBuilder.SetupContainer(args);
        containerBuilder.RegisterModule(new RestModule());
        
        IContainer container = containerBuilder.Build();

        foreach (IEndpointMapper mapper in container.Resolve<IEnumerable<IEndpointMapper>>())
        {
            mapper.MapEndpoint();
        }
        
        container.Resolve<WebApplication>().Run();
    }
}