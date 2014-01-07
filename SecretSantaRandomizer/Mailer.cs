using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using SecretSantaRandomizer.Configuration;

namespace SecretSantaRandomizer
{
	public class Mailer
	{
		private readonly MailServerSettings mailServerSettings;
		private readonly SmtpClient smtpClient;

		public Mailer()
		{
			mailServerSettings = new MailServerSettings(@"Settings\mailServerSettings");
			smtpClient = new SmtpClient(mailServerSettings.Server, 25)
				{
					DeliveryMethod = SmtpDeliveryMethod.Network,
					//TODO: extract to settings 
					Timeout = 30000,
					//EnableSsl = true,
					Credentials = new NetworkCredential(mailServerSettings.Login, mailServerSettings.Password)
				};
		}

		public void Send(string address, string subject, string htmlBody, string fileName = null)
		{
			using (var message = new MailMessage(mailServerSettings.Login, string.Format("<{0}>", address)))
			{
				message.BodyEncoding = Encoding.UTF8;
				message.SubjectEncoding = Encoding.UTF8;
				message.Subject = subject;
				message.Body = htmlBody;
				message.IsBodyHtml = true;
				if (fileName != null)
					message.Attachments.Add(new Attachment(fileName));
				smtpClient.Send(message);
			}
		}

		/// <summary>
		/// Template must/can contain {0} for the Santa name, {1} for the Santa's email, {2} for the Target name and {3} for the Target's email.
		/// </summary>
		/// <param name="pairs"></param>
		/// <param name="messageTemplate"></param>
		public void SendEmailsToPlayers(IEnumerable<Tuple<Player, Player>> pairs, string messageTemplate)
		{
			if (pairs == null)
				throw new ArgumentNullException("pairs");
			if (string.IsNullOrEmpty(messageTemplate))
				throw new ArgumentNullException("messageTemplate");

			const string messageSubject = "Secret Santa's New Mission!";
			foreach (var pair in pairs)
			{
				var asset = pair.Item1;
				var target = pair.Item2;
				Send(asset.Email, messageSubject, string.Format(messageTemplate, asset.Name, asset.Email, target.Name, target.Email));
				Thread.Sleep(500); //Not to look like we're going to spam the whole world;
			}
		}
	}
}