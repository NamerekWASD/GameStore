namespace API.Models.Games
{
	public class CopyModel
	{
		public int Id { get; set; }
		public string Data { get; set; } = string.Empty;
		public GameLightModel? Game { get; set; }
	}
}
