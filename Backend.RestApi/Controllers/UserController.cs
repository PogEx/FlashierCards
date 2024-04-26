using Backend.Common.Contracts;

namespace RestApiBackend.Controllers;

public class UserController: IEndpointMapper
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public UserController(IEndpointRouteBuilder routeBuilder)
    {
        _routeBuilder = routeBuilder;
    }
    public void MapEndpoint()
    {
       
    }
}