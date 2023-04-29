using BLL.DTO;

namespace BLL.Service.Genres
{
	public interface IGenreService
	{
		public IAsyncEnumerable<GenreDTO> GetGenres(CancellationToken cancellationToken);
	}
}