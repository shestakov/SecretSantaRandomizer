using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SecretSantaRandomizer;

namespace SecretSantaRandomizer_Tests
{
	[TestFixture]
	[Ignore]
	public class MailerTest
	{
		[SetUp]
		public void SetUp()
		{
			mailer = new Mailer();
		}

		private Mailer mailer;

		[Test]
		public void TestSend()
		{
			mailer.Send("login1@domain.ru", "Test", "From Santa");
		}

		[Test]
		public void TestSendEmailToPlayers()
		{
			var pairs = new List<Tuple<Player, Player>>
				{
					new Tuple<Player, Player>(new Player {Name = "login2", Email = "login2@domain.ru"}, new Player {Name = "login3", Email = "login3@domain.ru"}),
					new Tuple<Player, Player>(new Player {Name = "login3", Email = "login3@domain.ru"}, new Player {Name = "login2", Email = "login2@domain.ru"}),
				};
			mailer.SendEmailsToPlayers(pairs, new StreamReader(@"Settings\SecretSantaRandomizer.MessageTemplate.html").ReadToEnd());
		}

		[Test]
		public void TestSendEmailWithAttachment()
		{
			mailer.Send("login1@domain.ru", "TestWithAttachment", "Email text", @"Settings\attachmentExample.txt");
		}
	}
}