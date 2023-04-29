using API.Models.Games;

namespace API.Models
{
	public class CopyModel
	{
		public int Id { get; set; }
		public string Data { get; set; } = string.Empty;
		public int GameId { get; set; }
		public GameLightModel? Game { get; set; }
	}
}