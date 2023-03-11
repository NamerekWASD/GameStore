using DAL.Entity.Games;

namespace DAL.Entity.GameType
{
    public class CopyType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual Platform? Platform { get; set; }
        public virtual List<Region>? AvailableRegions { get; set; }
        public virtual List<Game>? Games { get; set; }
    }
}