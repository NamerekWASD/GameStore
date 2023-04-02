using DAL.Entity.Games;

namespace DAL.Entity.Mails
{
	public class GameSubscription
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public virtual Game? Game { get; set; }
		public string Email { get; set; } = string.Empty;
	}
}