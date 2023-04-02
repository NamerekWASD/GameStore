using BLL.DTO.Gernres;

namespace BLL.Service.Genres
{
	public interface IGenreService
	{
		public IAsyncEnumerable<GenreDTO> GetGenres(CancellationToken cancellationToken);
	}
}