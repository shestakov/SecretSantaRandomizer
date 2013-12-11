using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace SecretSantaRandomizer
{
	public class Mailer
	{
		private readonly SmtpClient smtpClient;
		private readonly string emailFrom;
		public Mailer()
		{
			smtpClient = new SmtpClient("smtp.mail.com", 25)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Timeout = 30000,
				//EnableSsl = true,
				Credentials = new NetworkCredential("SecretSantaAgency", "*********")
			};
			emailFrom = "<SecretSantaAgency@mail.com>";
		}

		public void Send(string address, string subject, string htmlBody)
		{
			using (var message = new MailMessage(emailFrom, address))
			{
				message.BodyEncoding = Encoding.UTF8;
				message.SubjectEncoding = Encoding.UTF8;
				message.Subject = subject;
				message.Body = htmlBody;
				message.IsBodyHtml = true;
				smtpClient.Send(message);
			}
		}

		public void SendAttachment(string address, string subject, string fileName)
		{
			using (var message = new MailMessage(emailFrom, address))
			{
				message.BodyEncoding = Encoding.UTF8;
				message.SubjectEncoding = Encoding.UTF8;
				message.Subject = subject;
				message.IsBodyHtml = true;
				message.Attachments.Add(new Attachment(fileName));
				smtpClient.Send(message);
			}
		}

		/// <summary>
		/// Template must/can contain {0} for the Santa name, {1} for the Santa's email, {2} for the Target name and {3} for the Target's email.
		/// </summary>
		/// <param name="pairs"></param>
		/// <param name="messageTemplate"></param>
		public void SendSecretSantaSpam(IEnumerable<Tuple<Player, Player>> pairs, string messageTemplate)
		{
			if (pairs == null)
				throw new ArgumentNullException("pairs");
			if (messageTemplate == null)
				throw new ArgumentNullException("messageTemplate");

			foreach (var pair in pairs)
			{
				const string messageSubject = "Secret Santa's New Mission!";
				var asset = pair.Item1;
				var target = pair.Item2;
				var formattedAddress = string.Format("<{0}>", asset.Email);
				Send(formattedAddress, messageSubject, string.Format(messageTemplate, asset.Name, asset.Email, target.Name, target.Email));
				Thread.Sleep(500); //Not to look like we're going to spam the whole world;
			}
		}
	}
}
