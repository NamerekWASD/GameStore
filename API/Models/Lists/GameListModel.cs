using API.Models.Games;

namespace API.Models.Lists
{
	public class GameListModel
	{
		public List<GameDetails> Games { get; set; } = new List<GameDetails>();
		public bool IsMax { get; set; }
		public int TotalCount { get; set; }
		public int Page { get; set; }
	}
}