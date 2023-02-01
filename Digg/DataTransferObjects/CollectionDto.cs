namespace Digg.DataTransferObjects
{
    public class CollectionDto<T> : DtoBase where T : DtoBase
    {
        public List<T> Records { get; set; } = new List<T>();
    }
}
