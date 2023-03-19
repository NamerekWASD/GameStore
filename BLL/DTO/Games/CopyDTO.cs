using DAL.Entity;

namespace BLL.DTO.Games
{
	public class CopyDTO
	{
		public int Id { get; set; }
		public string Data { get; set; } = string.Empty;
		public int GameId { get; set; }
		public GameDTO? Game { get; set; }
		public bool IsSold { get; set; } = false;
	}
}