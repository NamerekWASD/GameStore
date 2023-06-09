﻿

using API.Models.Games;

namespace API.Models
{
	public class GameCopyModel
	{
		public int Id { get; set; }
		public string Type { get; set; } = string.Empty;
		public string Data { get; set; } = string.Empty;
		public int GameId { get; set; }
		public GameLightModel? Game { get; set; }
	}
}