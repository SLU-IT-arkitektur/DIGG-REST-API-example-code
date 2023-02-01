namespace Digg.Models
{
    public class Range
    {
        public Range(int id, int min, int max)
        {
            Id = id;
            Min = min;
            Max = max;
        }

        public int Id { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
    }
}
