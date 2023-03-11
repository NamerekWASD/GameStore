using BLL.DTO.GameType;
using System.Text;

namespace BLL.DTO.Games
{
    public class GameDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string ImageURL { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<GenreDTO>? Genres { get; set; }
		public DeveloperDTO? Developer { get; set; }
		public PublisherDTO? Publisher { get; set; }
		public CopyTypeDTO? CopyType { get; set; }
		public List<CopyDTO>? Copies { get; set; }
		public DateTime Released { get; set; }
		public decimal Price { get; set; }
		public int Sold { get; set; }

		public string[] GetRegions
		{
			get
			{
				return CopyType.AvailableRegions.Select(item => item?.Name).ToArray();
			}
		}
		public string[] GetGenres
		{
			get
			{
				return Genres?.Select(g => g.Name).ToArray() ?? Array.Empty<string>();
			}
		}
	}
}
