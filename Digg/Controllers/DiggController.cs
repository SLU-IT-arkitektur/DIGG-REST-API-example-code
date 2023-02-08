namespace Digg.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    public class DiggController : ApiControllerBase
    {
        private readonly IApiVersionDescriptionProvider _apiVersionProvider;

        private readonly OpenApiInfo _openApiInfo;

        public DiggController(IApiVersionDescriptionProvider apiVersionProvider, IOptions<OpenApiInfo> options) 
        {
            _openApiInfo = options.Value;
            _apiVersionProvider = apiVersionProvider;
        }

        [HttpGet]
        [Route("dates/{dateTime:DateTime}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CamelCaseDto))]
        public IActionResult TestDateFormat(DateTime dateTime)
        {
            var utcNow = DateTime.UtcNow;

            return Ok(new CamelCaseDto()
            {
                IdExample = 1,
                NameExample = "Camel case property names.",
                DateTimeExample = dateTime,
                DateTimeStringExample = dateTime.ToString("o"),
                UtcNowDateTimeExample = utcNow,
                UtcNowDateTimeStringExample = utcNow.ToString("O")
            });
        }

        [HttpGet]
        [Route("errors")]
        public IActionResult TriggerError()
        {
            throw new Exception("Exception!");
        }

        [HttpGet]
        [Route("validationerrors")]
        public IActionResult TriggerValidationError()
        {
            ModelState.AddModelError(
                "Key".ToLowerInvariant(), 
                "Custom validation error".ToLowerInvariant());

            return ValidationProblem();
        }

        [HttpGet]
        [Route("caches")]
        [ResponseCache(CacheProfileName = "30sec")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult TestCache()
        {
            return Ok(DateTime.Now.ToString());
        }

        [HttpGet]
        [Route("apiinfo")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiInfoDto))]
        public IActionResult GetApiInfo(ApiVersion requestedApiVersion)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDate = System.IO.File.GetLastWriteTime(assembly.Location).ToShortDateString();

            var isMajorVersionDeprecated = 
                _apiVersionProvider.IsDeprecated(ControllerContext.ActionDescriptor, requestedApiVersion);

            return Ok(new ApiInfoDto() {
                ApiName = _openApiInfo.Title,
                ApiDocumentation = _openApiInfo.Description,
                ApiVersion = requestedApiVersion.ToString(),
                ApiReleased = assemblyDate,
                ApiStatus = isMajorVersionDeprecated ? "deprecated" : "active"
            });
        }
    }
}
