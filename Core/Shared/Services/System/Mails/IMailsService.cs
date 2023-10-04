using MimeKit;

namespace Core.Shared.Services.System.Mails
{
	public interface IMailsService
	{
		Task SendEmail(string to, string subject, string body);
		Task SendEmail(List<string> tos, string subject, string body);
	}
}
