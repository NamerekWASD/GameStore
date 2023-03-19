using API.Models.Games;
using AutoMapper;
using BLL.DTO.Games;
using BLL.DTO.Images;
using API.Models.Images;
using API.Models.Orders;
using BLL.DTO.Orders;
using Braintree;
using API.Models.Filters;
using BLL.DTO.Filters;
using BLL.DTO.GameType;
using API.Models.GameType;
using API.Models.Copies;

internal static class MapperHelpers
{

	public static IMapper Instance = new MapperConfiguration(cfg =>
	{
		cfg.CreateMap<GameDTO, GameLightModel>()
		.ForMember(destination => destination.CopyType, option => option.MapFrom(source => source.CopyType.Name))
		.ForMember(destination => destination.Platform, option => option.MapFrom(source => source.CopyType.Platform.Name))
		.ForMember(destination => destination.CopyCount, option => option.MapFrom(source => source.Copies.Where(item => !item.IsSold).Count()))
		.ForMember(destination => destination.Genres, option => option.MapFrom(source => source.GetGenres))
		.ForMember(destination => destination.Image, option => option.MapFrom(source => source.GetFirstPortrait));
		cfg.CreateMap<GameDTO, GameDetails>()
		.ForMember(destination => destination.Publisher, option => option.MapFrom(source => source.Publisher.Name))
		.ForMember(destination => destination.Developer, option => option.MapFrom(source => source.Developer.Name))
		.ForMember(destination => destination.CopyType, option => option.MapFrom(source => source.CopyType.Name))
		.ForMember(destination => destination.Platform, option => option.MapFrom(source => source.CopyType.Platform.Name))
		.ForMember(destination => destination.Regions, option => option.MapFrom(source => source.GetRegions))
		.ForMember(destination => destination.CopyCount, option => option.MapFrom(source => source.Copies.Where(item => !item.IsSold).Count()))
		.ForMember(destination => destination.Released, option => option.MapFrom(source => DateOnly.FromDateTime(source.Released)))
		.ForMember(destination => destination.Genres, option => option.MapFrom(source => source.GetGenres));
		cfg.CreateMap<OrderLightDTO, OrderLightModel>().ReverseMap();
		cfg.CreateMap<GameOrderDTO, GameOrderModel>().ReverseMap();
		cfg.CreateMap<BillingAddressDTO, BillingAddressModel>().ReverseMap();
		cfg.CreateMap<OrderDTO, OrderModel>();
		cfg.CreateMap<SoldCopyDTO, SoldCopyModel>();
		cfg.CreateMap<CopyDTO, CopyModel>();
		cfg.CreateMap<ImageDTO, ImageModel>().ReverseMap();
		cfg.CreateMap<BillingAddressModel, AddressRequest>();
		cfg.CreateMap<FilterFormDTO, FilterFormDataModel>();
		cfg.CreateMap<GenreDTO, GenreModel>().ReverseMap();
		cfg.CreateMap<PlatformDTO, PlatformModel>().ReverseMap();
		cfg.CreateMap<DeveloperDTO, DeveloperModel>().ReverseMap();
		cfg.CreateMap<PublisherDTO, PublisherModel>().ReverseMap();
		cfg.CreateMap<RegionDTO, RegionModel>().ReverseMap();
		cfg.CreateMap<GameDTO, GameModel>().ReverseMap();
		cfg.CreateMap<CopyTypeDTO, CopyTypeModel>().ReverseMap();

	}).CreateMapper();
}