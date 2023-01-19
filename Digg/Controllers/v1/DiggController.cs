namespace Digg.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DiggController : ApiControllerBase
    {
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CamelCaseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status409Conflict)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Index()
        {
            var utcNow = DateTime.UtcNow;

            return Ok(new CamelCaseExample()
            {
                IdExample = 1,
                NameExample = "camelCase!",
                DateTimeExample = utcNow,
                DateTimeStringExample = utcNow.ToString("o")
            });
        }

        [HttpGet]
        [Route("errors")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status409Conflict)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Errors()
        {
            throw new Exception("Exception!");
        }

        [HttpGet]
        [Route("validationerrors")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status409Conflict)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult ValidationErrors()
        {
            ModelState.AddModelError(
                "Key".ToLowerInvariant(), 
                "Custom validation error".ToLowerInvariant());

            return ValidationProblem();
        }
    }
}
