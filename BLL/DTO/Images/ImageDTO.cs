
namespace BLL.DTO
{
	public class ImageDTO
	{
		public int Id { get; set; }
		public int TypeId { get; set; }
		public ImageTypeDTO? Type { get; set; }
		public string Path { get; set; } = string.Empty;
		public string? ActualPath { get; set; }
		public int GameId { get; set; }
		public GameDTO? Game { get; set; }
	}
}