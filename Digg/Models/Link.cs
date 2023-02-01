namespace Digg.Models
{
    public class Link
    {
        public Link(string? href, string? rel, string? method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string? Href { get; private set; }
        public string? Rel { get; private set; }
        public string? Method { get; private set; }
    }
}
