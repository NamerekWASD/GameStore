namespace API.Models.Images
{
	public class ImageFormModel
	{
		public string FileName { get; set; }
		public IFormFile Image { get; set; }
	}
}
