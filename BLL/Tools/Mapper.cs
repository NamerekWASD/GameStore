using AutoMapper;
using BLL.DTO;
using BLL.DTO.Cheques;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using DAL.Entity.Cheques;
using DAL.Entity.Games;
using DAL.Entity.GameType;

namespace BLL.Tools
{
    internal static class Mapper
	{

		public static IMapper Instance = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<Cheque, ChequeDTO>().ReverseMap();
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

		}).CreateMapper();
	}
}
