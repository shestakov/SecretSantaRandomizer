using System;
using System.Collections.Generic;
using System.Linq;
using SecretSantaRandomizer.Exceptions;

namespace SecretSantaRandomizer
{
	public static class SantaRandomizer
	{
		public static Tuple<Player, Player>[] MakePairs(List<Player> players)
		{
			while (true)
				try
				{
					return AssignPairs(players).ToArray();
				}
				catch (SantaTargetingException)
				{
					Console.WriteLine("Oups, failed to pair assets and targets this time, selftargeting detected!");
				}
		}

		private static IEnumerable<Tuple<Player, Player>> AssignPairs(List<Player> players)
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
	}
}