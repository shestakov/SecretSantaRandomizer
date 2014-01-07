namespace SecretSantaRandomizer.Exceptions
{
	public class IncorrectMailingSettingsException : IncorrectSettingsException
	{
		public IncorrectMailingSettingsException() : base("Некорректные настройки рассылки")
		{
		}
	}
}