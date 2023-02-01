namespace Digg.Controllers.v1
{
    [ApiVersion("1.0")]
    public class RangesController : ApiControllerBase
    {
        private readonly IEnumerable<Models.Range> _ranges;

        public RangesController()
        {
            _ranges = new[]
            {
                new Models.Range(1, int.MinValue, int.MaxValue),
                new Models.Range(2, short.MinValue, short.MaxValue),
                new Models.Range(3, byte.MinValue, byte.MaxValue)
            };
        }

        [HttpGet]
        [Route("", Name = nameof(GetRanges))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CollectionDto<RangeDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult GetRanges()
        {
            return Ok(ToDto(_ranges));
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetRange))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RangeDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult GetRange(int id)
        {         
            var range = _ranges.Where(r => r.Id == id).FirstOrDefault();

            if (range == null)
            {
                return NotFound();
            }

            return Ok(ToDto(range));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(DeleteRange))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult DeleteRange(int id)
        {
            var range = _ranges.Where(r => r.Id == id).FirstOrDefault();

            if (range == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        protected RangeDto ToDto(Models.Range range)
        {
            var dto = new RangeDto();
            dto.Id = range.Id;
            dto.Min = range.Min;
            dto.Max = range.Max;

            dto.Links.Add(UrlLink("self", nameof(GetRange), new { id = range.Id }));
            dto.Links.Add(UrlLink("delete", nameof(DeleteRange), new { id = range.Id }));

            return dto;
        }

        protected CollectionDto<RangeDto> ToDto(IEnumerable<Models.Range> ranges)
        {
            var dto = new CollectionDto<RangeDto>();
            dto.Records = ranges.Select(r => ToDto(r)).ToList();

            dto.Links.Add(UrlLink("self", nameof(GetRanges), null));

            return dto;
        }
    }
}
