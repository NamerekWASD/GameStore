using BLL.DTO.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Lists
{
	public class GameListDTO
	{
		public List<GameDTO> Games { get; set; } = new List<GameDTO>();
		public bool IsMax { get; set; }
		public int TotalCount { get; set; }
		public int Page { get; set; }
	}
}
