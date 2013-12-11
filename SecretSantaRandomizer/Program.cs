using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretSantaRandomizer.JetBrainsAnnotations;

namespace SecretSantaRandomizer
{
	class Program
	{
		private const string AssetsandtargetsFileName = "AssetsAndTargets.txt";
		private const string GameMasterEmail = "game.master@mail.com";

		static void Main()
		{
			var players = LoadAndRandomlySortPlayers("SecretSanta.txt");
			var success = false;
			IEnumerable<Tuple<Player, Player>> pairs = null;
			while (!success)
			{
				try
				{
					pairs = MakePairs(players);
					SaveResult(pairs, AssetsandtargetsFileName);
					success = true;
				}
				catch(SantaTargetingException)
				{
					Console.WriteLine("Oups, failed to pair assets and targets this time, selftargeting detected!");
				}
			}

			var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SecretSantaRandomizer.MessageTemplate.html");
			var messageTemplate = new StreamReader(templateStream).ReadToEnd();
			SendEmailToGameMaster(pairs, GameMasterEmail);
			SendEmailsToPlayer(pairs, messageTemplate);
		}

		private static void SendEmailToGameMaster(IEnumerable<Tuple<Player, Player>> pairs, string gameMasterAddress)
		{
			if (pairs == null)
				throw new ArgumentNullException("pairs");
			var mailer = new Mailer();
			mailer.SendAttachment(gameMasterAddress, "Secret Santa Assets and Targets", AssetsandtargetsFileName);
			File.Delete(AssetsandtargetsFileName);
		}

		private static void SendEmailsToPlayer(IEnumerable<Tuple<Player, Player>> pairs, string messageTemplate)
		{
			if(pairs == null)
				throw new ArgumentNullException("pairs");
			var mailer = new Mailer();
			mailer.SendSecretSantaSpam(pairs, messageTemplate);
		}

		private static void SaveResult(IEnumerable<Tuple<Player, Player>> pairs, string fileName)
		{
			if (pairs == null)
				throw new ArgumentNullException("pairs");

			var builder = new StringBuilder();
			foreach (var pair in pairs)
			{
				builder.AppendLine(string.Format("{0}\t{1}\t{2}", pair.Item1.Name, pair.Item1.Email, pair.Item2.Name));
			}
			File.WriteAllText(fileName, builder.ToString(), Encoding.UTF8);
		}

		[NotNull]
		private static IEnumerable<Tuple<Player, Player>> MakePairs(List<Player> players)
		{
			var pairs = new List<Tuple<Player, Player>>();
			var assets = new List<Player>(players);
			var targets = new List<Player>(players);
			foreach (var asset in assets)
			{
				var count = targets.Count;
				if (count == 1 && targets[0].Id == asset.Id)
					throw new SantaTargetingException();
				var random = new Random();
				Player target = null;
				while (target == null)
				{
					var next = random.Next(0, count);
					if (targets[next].Id != asset.Id)
						target = targets[next];
				}
				pairs.Add(new Tuple<Player, Player>(asset, target));
				targets.Remove(target);
			}
			return pairs;
		}

		private static List<Player> LoadAndRandomlySortPlayers(string fileName)
		{
			var lines = File.ReadAllLines(fileName, Encoding.UTF8);
			var players = new List<Player>();
			foreach (var line in lines)
			{
				var parts = line.Split('\t');
				if (parts.Length != 2)
					throw new Exception(string.Format("Incorrect name-email pair: {0}", line));
				
				players.Add(new Player { Id = Guid.NewGuid().ToString("N"), Name = parts[0], Email = parts[1] });
			}
			return players.OrderBy(p => p.Id).ToList();
		}
	}
}