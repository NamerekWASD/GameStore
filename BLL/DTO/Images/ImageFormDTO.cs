﻿using Microsoft.AspNetCore.Http;

namespace BLL.DTO
{
	public class ImageFormDTO
	{
		public int TypeId { get; set; }
		public string Type { get; set; } = string.Empty;
		public IFormFile? Image { get; set; }
	}
}