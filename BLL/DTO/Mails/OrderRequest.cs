namespace BLL.DTO.Mails
{
	public class OrderRequest
	{
		public string ToEmail { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
	}
}