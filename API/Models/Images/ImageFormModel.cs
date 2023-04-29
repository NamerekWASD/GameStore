namespace API.Models
{
	public class ImageFormModel
	{
		public int TypeId { get; set; }
		public string Type { get; set; } = string.Empty;
		public IFormFile? Image { get; set; }
	}
}