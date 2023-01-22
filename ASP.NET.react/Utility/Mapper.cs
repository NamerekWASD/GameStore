using ASP.NET.react.Models;
using AutoMapper;
using BLL.DTO;

namespace ASP.NET.react.Utility
{
	public static class Mapper
	{
		public static IMapper mapperConfiguration = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<CompanyModel, CompanyDTO>();
		}).CreateMapper();

	}
}