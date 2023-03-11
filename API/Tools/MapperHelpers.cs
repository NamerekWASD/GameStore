using API.Models.Games;
using AutoMapper;
using BLL.DTO.Games;
using BLL.DTO.Cheques;
using API.Models.Cheques;
using BLL.DTO;
using DAL.Entity;
using DAL.Entity.Games;

internal static class MapperHelpers
{

	public static IMapper Instance = new MapperConfiguration(cfg =>
	{
		cfg.CreateMap<GenreDTO, GenreModel>().ReverseMap();
		cfg.CreateMap<GameDTO, GameLightModel>()
		.ForMember(destination => destination.CopyType, option => option.MapFrom(source => source.CopyType.Name))
		.ForMember(destination => destination.Platform, option => option.MapFrom(source => source.CopyType.Platform.Name))
		.ForMember(destination => destination.CopyCount, option => option.MapFrom(source => source.Copies.Where(item => !item.IsSold).Count()));
		cfg.CreateMap<GameDTO, GameDetails>()
		.ForMember(destination => destination.Publisher, option => option.MapFrom(source => source.Publisher.Name))
		.ForMember(destination => destination.Developer, option => option.MapFrom(source => source.Developer.Name))
		.ForMember(destination => destination.CopyType, option => option.MapFrom(source => source.CopyType.Name))
		.ForMember(destination => destination.Platform, option => option.MapFrom(source => source.CopyType.Platform.Name))
		.ForMember(destination => destination.Regions, option => option.MapFrom(source => source.GetRegions))
		.ForMember(destination => destination.CopyCount, option => option.MapFrom(source => source.Copies.Where(item => !item.IsSold).Count()))
		.ForMember(destination => destination.Released, option => option.MapFrom(source => DateOnly.FromDateTime(source.Released)))
		.ForMember(destination => destination.Genres, option => option.MapFrom(source => source.GetGenres));
		cfg.CreateMap<ChequeLightDTO, ChequeLightModel>().ReverseMap();
		cfg.CreateMap<GameChequeDTO, GameChequeModel>().ReverseMap();
		cfg.CreateMap<BillingAddressDTO, BillingAddressModel>().ReverseMap();
		cfg.CreateMap<ChequeDTO, ChequeModel>();
		cfg.CreateMap<SoldCopyDTO, SoldCopyModel>();
		cfg.CreateMap<CopyDTO, CopyModel>();

	}).CreateMapper();
}