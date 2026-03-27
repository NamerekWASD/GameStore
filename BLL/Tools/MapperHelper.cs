using AutoMapper;
using BLL.DTO;
using BLL.DTO.Mails;
using BLL.DTO.Orders;
using Braintree;
using DAL.Entity;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Tools
{
	public static class MapperHelpers
    {
        private static readonly MapperConfiguration _config = new MapperConfiguration(cfg => { SetupConfiguration(cfg); }, NullLoggerFactory.Instance);
        public static readonly IMapper Instance = _config.CreateMapper();

		private static void SetupConfiguration(IMapperConfigurationExpression cfg)
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
            cfg.CreateMap<BillingAddressDTO, AddressRequest>();
            cfg.CreateMap<SoldCopy, SoldCopyDTO>().ReverseMap();
			cfg.CreateMap<GameSubscription, GameSubscriptionDTO>().ReverseMap();
			cfg.CreateMap<Image, ImageDTO>().ReverseMap();
			cfg.CreateMap<ImageTypeDTO, ImageType>().ReverseMap();
			cfg.CreateMap<Tag, TagDTO>().ReverseMap();
		}
	}
}