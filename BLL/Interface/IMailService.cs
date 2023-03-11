using BLL.DTO;

namespace BLL.Interface
{
	public interface IMailService
	{
		Task SendEmailAsync(MailRequest mailRequest);
	}
}
