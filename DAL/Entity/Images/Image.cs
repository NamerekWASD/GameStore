using DAL.Entity.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity.Images
{
	public class Image
	{
		public int Id { get; set; }
		public int TypeId { get; set; }
		public virtual ImageType? Type { get; set; }
		public string Path { get; set; } = string.Empty;
		public string? ActualPath { get; set; }
		public int? GameId { get; set; }
		public virtual Game? Game { get; set; }
	}
}
