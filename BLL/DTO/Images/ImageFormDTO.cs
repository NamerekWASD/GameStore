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
		public string FileName { get; set; }
		public IFormFile Image { get; set; }
	}
}
