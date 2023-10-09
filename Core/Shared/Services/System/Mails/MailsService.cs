using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Core.Shared.Services.System.Mails;

public class MailsService : IMailsService
{
	public async Task SendEmail(string to, string subject, string body)
	{
		MimeMessage email = new();

		email.To.Add(MailboxAddress.Parse(to));

		await Execute(email, subject, body);
	}

	public async Task SendEmail(List<string> tos, string subject, string body)
	{
		MimeMessage email = new();

		foreach (string to in tos) email.To.Add(MailboxAddress.Parse(to));

		await Execute(email, subject, body);
	}

	private static async Task Execute(MimeMessage email, string subject, string body)
	{
		email.From.Add(MailboxAddress.Parse("noreply@eks-ekidi.com"));
		email.Subject = subject;
		email.Body = new TextPart(TextFormat.Plain) { Text = body };

		using SmtpClient smtp = new();

		//smtp.Connect("smtp.ionos.fr", 465, SecureSocketOptions.Auto); // ok local
		smtp.Connect("smtp.ionos.fr", 587, SecureSocketOptions.StartTls); // ok local
		//smtp.Connect("smtp.ionos.fr", 25, SecureSocketOptions.None);

		smtp.Authenticate("noreply@eks-ekidi.com", "noreply2022");

		try
		{
			await smtp.SendAsync(email);
		}
		catch (Exception e)
		{
			throw new Exception("Error while sending email check if the email is valid");
		}

		smtp.Disconnect(true);
	}
}