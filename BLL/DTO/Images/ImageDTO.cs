using BLL.DTO.Games;

namespace BLL.DTO.Images
{
	public class ImageDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public int GameId { get; set; }
		public GameDTO? Game { get; set; }
	}
}
