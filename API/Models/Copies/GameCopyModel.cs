using API.Models.Games;
using BLL.DTO.Games;

namespace API.Models.Copies
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
