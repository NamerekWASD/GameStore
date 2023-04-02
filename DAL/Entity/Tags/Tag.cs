using DAL.Entity.Games;

namespace DAL.Entity.Tags
{
	public class Tag
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public virtual List<Game>? Games { get; set; }
	}
}