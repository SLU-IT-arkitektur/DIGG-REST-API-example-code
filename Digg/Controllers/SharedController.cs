namespace Digg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SharedController : ControllerBase
    {
        [Route("error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var exception =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            var traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Log errors here. Use traceId to connect exception log entries with
            // problem detail messages. For "custom trace IDs" the static method
            // Results.Problem with argument "IDictionary<string,object?>? extensions"
            // is available.

            return Problem();
        }           
    }
}
