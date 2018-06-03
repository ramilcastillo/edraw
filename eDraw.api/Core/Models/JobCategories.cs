namespace eDraw.api.Core.Models
{
    public class JobCategories
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long JobId { get; set; }
        public Jobs Job { get; set; }
    }
}
