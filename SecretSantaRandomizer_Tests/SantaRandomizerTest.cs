using System;
using System.Collections.Generic;
using NUnit.Framework;
using SecretSantaRandomizer;

namespace SecretSantaRandomizer_Tests
{
	[TestFixture]
	public class SantaRandomizerTest
	{
		[Test]
		public void TestMakePairs()
		{
			var players = new List<Player>
				{
					new Player {Id = "1", Name = "login1", Email = "login1@ya.ru"},
					new Player {Id = "2", Name = "login2", Email = "login2@ya.ru"},
					new Player {Id = "3", Name = "login3", Email = "login3@ya.ru"},
					new Player {Id = "4", Name = "login4", Email = "login4@ya.ru"},
					new Player {Id = "5", Name = "login5", Email = "login5@ya.ru"},
					new Player {Id = "6", Name = "login6", Email = "login6@ya.ru"},
					new Player {Id = "7", Name = "login7", Email = "login7@ya.ru"},
					new Player {Id = "8", Name = "login8", Email = "login8@ya.ru"},
					new Player {Id = "9", Name = "login9", Email = "login9@ya.ru"},
					new Player {Id = "10", Name = "login10", Email = "login10@ya.ru"}
				};
			var pairs = SantaRandomizer.MakePairs(players);
			foreach (var pair in pairs)
				Console.WriteLine("{0}\t{1}\t{2}\t{3}", pair.Item1.Name, pair.Item1.Email, pair.Item2.Name, pair.Item2.Email);
		}
	}
}