﻿using System.ComponentModel.DataAnnotations;

namespace ASP.NET.react.Models
{
	public class UserModel
	{
		[Required]
		public string UserName { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
		[Required]
		public string Name { get; set; } = string.Empty;
		public string Lastname { get; set; } = string.Empty;
	}
}
