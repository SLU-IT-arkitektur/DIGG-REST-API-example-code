[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public abstract class ApiControllerBase : ControllerBase 
{
    private IActionDescriptorCollectionProvider? _actionDescriptorCollectionProvider;

    public IActionDescriptorCollectionProvider ActionDescriptorCollectionProvider
    {
        get
        {
            if (_actionDescriptorCollectionProvider == null)
            {
                _actionDescriptorCollectionProvider = 
                    HttpContext?.RequestServices?.GetRequiredService<IActionDescriptorCollectionProvider>();
            }

            return _actionDescriptorCollectionProvider!;
        }
    }
    
    protected Link UrlLink(string relation, string routeName, object? values)
    {
        var routes = ActionDescriptorCollectionProvider.ActionDescriptors.Items;
        
        var route = routes.FirstOrDefault(r => routeName.Equals(r.AttributeRouteInfo?.Name));
        var method = route?.ActionConstraints?.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();

        var url = Url.Link(routeName, values)?.ToLower();

        return new Link(url, relation, method);
    }
}
