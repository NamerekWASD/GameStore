using BLL.DTO.Gernres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service.Genres
{
    public interface IGenreService
    {
        public IAsyncEnumerable<GenreDTO> GetGenres(CancellationToken cancellationToken);
    }
}
