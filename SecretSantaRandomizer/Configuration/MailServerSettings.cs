using System.IO;
using SecretSantaRandomizer.Exceptions;

namespace SecretSantaRandomizer.Configuration
{
	public class MailServerSettings
	{
		public MailServerSettings(string settingsFile)
		{
			using (var reader = new StreamReader(settingsFile))
			{
				var settings = reader.ReadToEnd().Split('\n');
				if (settings.Length != 3)
					throw new IncorrectMailServerSettingsException();
				Server = settings[0].Split('=')[1].Trim();
				Login = settings[1].Split('=')[1].Trim();
				Password = settings[2].Split('=')[1];
			}

			if (!IsValid())
				throw new IncorrectMailServerSettingsException();
		}

		public string Server { get; private set; }
		public string Login { get; private set; }
		public string Password { get; private set; }

		private bool IsValid()
		{
			return !string.IsNullOrEmpty(Server) && !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
		}
	}
}