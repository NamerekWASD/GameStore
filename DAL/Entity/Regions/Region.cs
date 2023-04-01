using DAL.Entity.GameType;

namespace DAL.Entity.Regions
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<CopyType>? CopyTypes { get; set; }
    }
}