namespace Digg.Controllers
{
    [ApiVersion("2.0")]
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
        public IActionResult GetRanges()
        {
            return Ok(ToDto(_ranges));
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetRange))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RangeDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public IActionResult GetRange(int id)
        {         
            var range = _ranges.Where(r => r.Id == id).FirstOrDefault();

            if (range == null)
            {
                return NotFound();
            }

            return Ok(ToDto(range));
        }

        [HttpPost]
        [Route("", Name = nameof(PostRange))]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(DtoBase))]
        public IActionResult PostRange()
        {         
            var dto = new DtoBase();
           
            AddLinks(dto, 1);

            return CreatedAtAction(nameof(GetRange), new { id = 1 }, dto);
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(DeleteRange))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
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

            AddLinks(dto, dto.Id);

            return dto;
        }

        protected CollectionDto<RangeDto> ToDto(IEnumerable<Models.Range> ranges)
        {
            var dto = new CollectionDto<RangeDto>();
            dto.Records = ranges.Select(r => ToDto(r)).ToList();

            AddCollectionLinks(dto);

            return dto;
        }

        protected void AddLinks(DtoBase dtoBase, int id)
        {
            dtoBase.Links.Add(UrlLink("self", nameof(GetRange), new { id }));
            dtoBase.Links.Add(UrlLink("delete", nameof(DeleteRange), new { id }));
        }

        protected void AddCollectionLinks(DtoBase dtoBase)
        {
            dtoBase.Links.Add(UrlLink("self", nameof(GetRanges), null));
        }
    }
}
