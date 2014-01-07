using System.IO;
using SecretSantaRandomizer.Exceptions;

namespace SecretSantaRandomizer.Configuration
{
	public class MailingSettings
	{
		public MailingSettings(string settingsFile)
		{
			using (var reader = new StreamReader(settingsFile))
			{
				var settings = reader.ReadToEnd().Split('\n');
				if (settings.Length != 3)
					throw new IncorrectMailingSettingsException();
				PlayersList = settings[0].Split('=')[1].Trim();
				GameMasterEmail = settings[1].Split('=')[1].Trim();
				MailTemplate = settings[2].Split('=')[1].Trim();
			}
			if (!IsValid())
				throw new IncorrectMailingSettingsException();
		}

		public string PlayersList { get; private set; }
		public string GameMasterEmail { get; private set; }
		public string MailTemplate { get; private set; }

		private bool IsValid()
		{
			return !string.IsNullOrEmpty(PlayersList) && !string.IsNullOrEmpty(GameMasterEmail) && !string.IsNullOrEmpty(MailTemplate);
		}
	}
}