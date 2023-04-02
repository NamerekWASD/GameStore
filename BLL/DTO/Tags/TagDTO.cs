using BLL.DTO.Games;

namespace BLL.DTO.Tags
{
	public class TagDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public List<GameDTO>? Games { get; set; }
	}
}