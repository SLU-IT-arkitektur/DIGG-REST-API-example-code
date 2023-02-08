using System.Text.Json.Serialization;

namespace Digg.DataTransferObjects
{
    public class DtoBase
    {
        [JsonPropertyName("_links")]
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
