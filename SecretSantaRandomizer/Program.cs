using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SecretSantaRandomizer.Configuration;

namespace SecretSantaRandomizer
{
	internal class Program
	{
		private const string AssetsandtargetsFileName = "AssetsAndTargets.txt";
		private static Tuple<Player, Player>[] _pairs = new Tuple<Player, Player>[0];
		private static Mailer _mailer;
		private static MailingSettings _mailingSettings;

		private static Mailer Mailer
		{
			get { return _mailer ?? (_mailer = new Mailer()); }
		}

		public static void Main()
		{
			_mailingSettings = new MailingSettings("mailingSettings");
			_pairs = SantaRandomizer.MakePairs(LoadPlayers());
			SendEmailToGameMaster();
			Mailer.SendEmailsToPlayers(_pairs, new StreamReader(_mailingSettings.MailTemplate).ReadToEnd());
		}

		private static void SendEmailToGameMaster()
		{
			using (var writter = new StreamWriter(AssetsandtargetsFileName, false, Encoding.UTF8))
				foreach (var pair in _pairs)
					writter.WriteLine("{0}\t{1}\t{2}", pair.Item1.Name, pair.Item1.Email, pair.Item2.Name);

			Mailer.Send(_mailingSettings.GameMasterEmail, "Secret Santa Assets and Targets", "Here's the list of player pairs.", AssetsandtargetsFileName);
			File.Delete(AssetsandtargetsFileName);
		}

		private static List<Player> LoadPlayers()
		{
			var players = new List<Player>();
			using (var reader = new StreamReader(_mailingSettings.PlayersList, Encoding.UTF8))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var parts = line.Split('\t');
					if (parts.Length != 2)
						throw new Exception(string.Format("Incorrect name-email pair: {0}", line));

					players.Add(new Player {Id = Guid.NewGuid().ToString("N"), Name = parts[0], Email = parts[1]});
				}
				return players;
			}
		}
	}
}