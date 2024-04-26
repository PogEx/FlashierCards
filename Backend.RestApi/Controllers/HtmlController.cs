using RestApiBackend.Contracts;

namespace RestApiBackend.Controllers;

public class HtmlController: IEndpointMapper
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public HtmlController(IEndpointRouteBuilder routeBuilder)
    {
        _routeBuilder = routeBuilder;
    }

    public void MapEndpoint()
    {
        
        RouteGroupBuilder htmlGroup = _routeBuilder.MapGroup("html");

        htmlGroup.MapGet("index", () =>
            {
                return "SomeText";
            })
            .WithName("Html Index")
            .WithOpenApi();
    }
}