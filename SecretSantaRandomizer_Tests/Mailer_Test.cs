using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SecretSantaRandomizer;

namespace SecretSantaRandomizer_Tests
{
	[TestFixture]
	public class Mailer_Test
	{
		[Test]
		public void Send_Test()
		{
			var mailer = new Mailer();
			mailer.Send("<shestakov@***kontur.ru>", "Test", "From Santa");
		}

		[Test]
		public void SendSecretSantaSpam_Test()
		{
			var mailer = new Mailer();
			var pairs = new List<Tuple<Player, Player>>
				{
					new Tuple<Player, Player>(new Player{Name = "Александр Шестаков", Email = "shestakov@***kontur.ru"}, new Player{Name = "Иван Кузнецов", Email = "john_smith@***kontur.ru"}),
					new Tuple<Player, Player>(new Player{Name = "Александр Шестаков", Email = "alex.shestakov@*mail.com"}, new Player{Name = "Кузнец Иванов", Email = "smith_john@***kontur.ru"})
				};
			var templateStream = GetType().Assembly.GetManifestResourceStream("SecretSantaRandomizer.MessageTemplate.html");
			string messageTemplate = new StreamReader(templateStream).ReadToEnd();
			mailer.SendSecretSantaSpam(pairs, messageTemplate);
		}
	}
}