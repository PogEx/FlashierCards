using Autofac;
using RestApiBackend.Contracts;
using RestApiBackend.IoC;

namespace RestApiBackend;

public class Program
{
    public static void Main(string[] args)
    {
        ContainerBuilder containerBuilder = new ();
        
        containerBuilder.SetupContainer();
        IContainer container = containerBuilder.Build();

        foreach (IEndpointMapper mapper in container.Resolve<IEnumerable<IEndpointMapper>>())
        {
            mapper.MapEndpoint();
        }
        
        container.Resolve<WebApplication>().Run();
    }
}