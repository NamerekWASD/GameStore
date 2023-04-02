using BLL.DTO.Copies;
using BLL.DTO.Developers;
using BLL.DTO.GameType;
using BLL.DTO.Gernres;
using BLL.DTO.Images;
using BLL.DTO.Mails;
using BLL.DTO.Publishers;
using BLL.DTO.Tags;
using BLL.Tools;

namespace BLL.DTO.Games
{
    public class GameDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public List<ImageDTO> Images { get; set; } = new();
		public string Description { get; set; } = string.Empty;
		public List<GenreDTO> Genres { get; set; } = new();
		public List<TagDTO> Tags { get; set; } = new();
		public int DeveloperId { get; set; }
		public DeveloperDTO? Developer { get; set; }
		public int PublisherId { get; set; }
		public PublisherDTO? Publisher { get; set; }
		public int? CopyTypeId { get; set; }
		public CopyTypeDTO? CopyType { get; set; }
		public List<CopyDTO> Copies { get; set; } = new();
		public DateTime? Released { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsHotOffer { get; set; }
		public decimal? Price { get; set; }
		public decimal? DiscountPrice { get; set; }
		public int SoldCopies { get; set; }
		public List<GameSubscriptionDTO> Subscriptions { get; set; } = new();

		public string[] GetRegions
		{
			get
			{
				if(CopyType is null) return Array.Empty<string>();
				return CopyType.AvailableRegions.Select(item => item.Name).ToArray();
			}
		}
		public string[] GetGenres
		{
			get
			{
				return Genres?.Select(g => g.Name).ToArray() ?? Array.Empty<string>();
			}
		}
		public string[] GetTags
		{
			get
			{
				return Tags?.Select(g => g.Name).ToArray() ?? Array.Empty<string>();
			}
		}

		public ImageDTO GetFirstPortrait
		{
			get
			{
				return Images?.FirstOrDefault(image => image.Type != null && image.Type.Name == Constants.PORTRAIT_IMAGE) ?? new();
			}
		}

	}
}
