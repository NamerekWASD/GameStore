namespace DAL.Entity.Games
{
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public virtual List<Game>? Games { get; set; }
	}
}