namespace BLL.DTO
{
	public class GameListDTO
	{
		public List<GameDTO> Games { get; set; } = new List<GameDTO>();
		public bool IsMax { get; set; }
		public int TotalCount { get; set; }
		public int Page { get; set; }
	}
}