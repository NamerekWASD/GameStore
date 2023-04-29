using BLL.DTO;

namespace BLL.DTO
{
	public class GenreDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public List<GameDTO> Games { get; set; } = new();
	}
}