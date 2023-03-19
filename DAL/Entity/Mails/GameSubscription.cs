using DAL.Entity.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity.Mails
{
	public class GameSubscription
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public virtual Game? Game { get; set; }
		public string Email { get; set; } = string.Empty;
	}
}
