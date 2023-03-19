namespace API.Models.Images
{
	public class ImageModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public int GameId { get; set; }

	}
}
