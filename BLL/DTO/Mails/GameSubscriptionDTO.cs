
namespace BLL.DTO.Mails
{
	public class GameSubscriptionDTO
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public GameDTO? Game { get; set; }
		public string Email { get; set; } = string.Empty;
	}
}