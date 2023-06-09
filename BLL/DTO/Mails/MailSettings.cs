﻿namespace BLL.DTO.Mails
{
	public class MailSettings
	{
		public string Mail { get; set; } = string.Empty;
		public string DisplayName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Host { get; set; } = string.Empty;
		public string Proxy { get; set; } = string.Empty;
		public int Port { get; set; }
	}
}