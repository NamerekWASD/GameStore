﻿
namespace DAL.Entity
{
	public class CopyType
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int PlatformId { get; set; }
		public virtual Platform? Platform { get; set; }
		public virtual List<Region> AvailableRegions { get; set; } = new();
		public virtual List<Game> Games { get; set; } = new();
	}
}