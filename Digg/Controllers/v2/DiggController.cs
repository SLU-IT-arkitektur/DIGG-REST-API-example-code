namespace Digg.Controllers.v2
{
    [ApiVersion("2.0")]
    public class DiggController : ApiControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Hello v2.0!";
        }
    }
}
