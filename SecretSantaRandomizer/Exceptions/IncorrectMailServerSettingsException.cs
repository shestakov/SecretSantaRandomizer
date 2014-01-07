namespace SecretSantaRandomizer.Exceptions
{
	public class IncorrectMailServerSettingsException : IncorrectSettingsException
	{
		public IncorrectMailServerSettingsException() : base("Некорректные настройки mail сервера рассылки")
		{
		}
	}
}