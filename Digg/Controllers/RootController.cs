namespace Digg.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}")]
    public class RootController : ApiControllerBase
    {
        private readonly IApiVersionDescriptionProvider _apiVersionProvider;

        private readonly OpenApiInfo _openApiInfo;

        public RootController(IApiVersionDescriptionProvider apiVersionProvider, IOptions<OpenApiInfo> options) 
        {
            _openApiInfo = options.Value;
            _apiVersionProvider = apiVersionProvider;
        }

        [HttpGet]
        [Route("apiinfo")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult ApiInfo(ApiVersion requestedApiVersion)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDate = System.IO.File.GetLastWriteTime(assembly.Location).ToShortDateString();

            var isMajorVersionDeprecated = _apiVersionProvider.IsDeprecated(ControllerContext.ActionDescriptor, requestedApiVersion);

            return Ok(new Digg.Models.ApiInfo() {
                ApiName = _openApiInfo.Title,
                ApiDocumentation = _openApiInfo.Description,
                ApiVersion = requestedApiVersion.ToString(),
                ApiReleased = assemblyDate,
                ApiStatus = isMajorVersionDeprecated ? "deprecated" : "active"
            });
        }
    }
}
