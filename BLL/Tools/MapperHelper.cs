using AutoMapper;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using BLL.DTO.Images;
using BLL.DTO.Mails;
using BLL.DTO.Orders;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Orders;

namespace BLL.Tools
{
	internal static class MapperHelper
	{

		public static IMapper Instance = new MapperConfiguration(cfg =>
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

		}).CreateMapper();
	}
}
