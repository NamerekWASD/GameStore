using DAL.Entity;

namespace DAL.Entity
{
	public class Developer
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public virtual List<Game>? Games { get; set; }
	}
}