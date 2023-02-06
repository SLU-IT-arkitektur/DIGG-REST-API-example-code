namespace Digg.Controllers
{
    [ApiVersion("1.0")]
    public class DiggController : ApiControllerBase
    {
        [HttpGet]
        [Route("dates/{dateTime:DateTime}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CamelCaseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult Index(DateTime dateTime)
        {
            var utcNow = DateTime.UtcNow;

            return Ok(new CamelCaseExample()
            {
                IdExample = 1,
                NameExample = "camelCase!",
                DateTimeExample = dateTime,
                DateTimeStringExample = dateTime.ToString("o"),
                UtcNowDateTimeExample = utcNow,
                UtcNowDateTimeStringExample = utcNow.ToString("O")
            });
        }

        [HttpGet]
        [Route("errors")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult Errors()
        {
            throw new Exception("Exception!");
        }

        [HttpGet]
        [Route("validationerrors")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult ValidationErrors()
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
        public IActionResult Caches()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}
