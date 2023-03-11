using BLL.DTO.Games;

namespace API.Models.Games
{
	public class GameCopyModel
	{
		public int Id { get; set; }
		public string Type { get; set; } = string.Empty;
		public string Data { get; set; } = string.Empty;
		public GameLightModel? Game { get; set; }
	}
}
