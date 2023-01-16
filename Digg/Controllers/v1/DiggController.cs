using Digg.Models;

namespace Digg.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DiggController : ApiControllerBase
    {
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status409Conflict)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        public CamelCaseExample Index(int id)
        {
            var utcNow = DateTime.UtcNow;

            return new CamelCaseExample()
            {
                IdExample = id,
                NameExample = "camelCase!",
                DateTimeExample = utcNow,
                DateTimeStringExample = utcNow.ToString("o")
            };
        }
    }
}
