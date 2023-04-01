using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Images
{
	public class ImageFormDTO
	{
		public int TypeId { get; set; }
		public string Type { get; set; } = string.Empty;
		public IFormFile Image { get; set; }
	}
}
