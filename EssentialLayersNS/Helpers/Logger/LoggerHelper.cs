using Serilog;
using System;

namespace EssentialLayers.Helpers.Logger
{
	public static class LoggerHelper
	{
		public static void Info(string message)
		{
			Log.Logger.Information(message);
		}

		public static void Error(Exception e, string message)
		{
			Log.Logger.Error(e, message);
		}

		public static void Warning(string message)
		{
			Log.Logger.Warning(message);
		}
	}
}