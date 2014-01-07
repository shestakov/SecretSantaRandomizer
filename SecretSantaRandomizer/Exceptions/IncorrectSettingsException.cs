using System;

namespace SecretSantaRandomizer.Exceptions
{
	public class IncorrectSettingsException : Exception
	{
		protected IncorrectSettingsException(string message) : base(message)
		{
		}
	}
}