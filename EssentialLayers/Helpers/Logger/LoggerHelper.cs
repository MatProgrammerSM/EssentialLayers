using Microsoft.Extensions.Logging;
using System;

namespace EssentialLayers.Helpers.Logger
{
	public static class LoggerHelper<T>
	{
		private static readonly ILogger<T> _logger;

		public static void Debug(Exception e, string message)
		{
			_logger.LogDebug(e, message);
		}

		public static void Error(Exception e, string message)
		{
			_logger.LogError(e, message);
		}

		public static void Info(string message)
		{
			_logger.LogInformation(message);
		}

		public static void Trace(Exception e, string message)
		{
			_logger.LogTrace(e, message);
		}

		public static void Warning(string message)
		{
			_logger.LogWarning(message);
		}
	}
}