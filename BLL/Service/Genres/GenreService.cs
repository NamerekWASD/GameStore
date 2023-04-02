using BLL.DTO.Gernres;
using BLL.Tools;
using DAL.Context;
using DAL.UoW;
using System.Runtime.CompilerServices;
using UnitsOfWork.Interfaces;

namespace BLL.Service.Genres
{
	public class GenreService : IGenreService
	{
		private readonly IUnitOfWork _unitOfWork;

		public GenreService(GameContext context)
		{
			_unitOfWork = new UnitOfWork(context);
		}

		public async IAsyncEnumerable<GenreDTO> GetGenres([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var genre in _unitOfWork.Genres.GetAll(cancellationToken))
			{
				yield return MapperHelper.Instance.Map<GenreDTO>(genre);
			}
		}
	}
}