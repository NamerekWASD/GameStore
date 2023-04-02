using AutoMapper;
using BLL.DTO.Copies;
using BLL.DTO.Developers;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using BLL.DTO.Gernres;
using BLL.DTO.Images;
using BLL.DTO.Mails;
using BLL.DTO.Orders;
using BLL.DTO.Platforms;
using BLL.DTO.Publishers;
using BLL.DTO.Regions;
using BLL.DTO.Tags;
using DAL.Entity.BillingAddresses;
using DAL.Entity.Copies;
using DAL.Entity.Developers;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Genres;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Orders;
using DAL.Entity.Platforms;
using DAL.Entity.Publishers;
using DAL.Entity.Regions;
using DAL.Entity.SoldCopies;
using DAL.Entity.Tags;

namespace BLL.Tools
{
	public static class MapperHelper
	{
		public static readonly IMapper Instance = new MapperConfiguration(SetupCofiguration).CreateMapper();

		private static void SetupCofiguration(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<Order, OrderDTO>().ReverseMap();
			cfg.CreateMap<Game, GameDTO>().ReverseMap();
			cfg.CreateMap<Developer, DeveloperDTO>().ReverseMap();
			cfg.CreateMap<Publisher, PublisherDTO>().ReverseMap();
			cfg.CreateMap<Platform, PlatformDTO>().ReverseMap();
			cfg.CreateMap<Genre, GenreDTO>().ReverseMap();
			cfg.CreateMap<CopyType, CopyTypeDTO>().ReverseMap();
			cfg.CreateMap<Copy, CopyDTO>().ReverseMap();
			cfg.CreateMap<Region, RegionDTO>().ReverseMap();
			cfg.CreateMap<BillingAddress, BillingAddressDTO>().ReverseMap();
			cfg.CreateMap<SoldCopy, SoldCopyDTO>().ReverseMap();
			cfg.CreateMap<GameSubscription, GameSubscriptionDTO>().ReverseMap();
			cfg.CreateMap<Image, ImageDTO>().ReverseMap();
			cfg.CreateMap<ImageTypeDTO, ImageType>().ReverseMap();
			cfg.CreateMap<Tag, TagDTO>().ReverseMap();
		}
	}
}