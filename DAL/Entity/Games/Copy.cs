using System.Diagnostics.CodeAnalysis;

namespace DAL.Entity.Games
{
	public class Copy
	{
		public int Id { get; set; }
		public string Data { get; set; } = string.Empty;
		public int GameId { get; set; }
		public virtual Game? Game { get; set; }
		public bool IsSold { get; set; } = false;
	}
}