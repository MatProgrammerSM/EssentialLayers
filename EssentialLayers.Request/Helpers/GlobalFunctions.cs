using EssentialLayers.Helpers.Extension;
using System;
using System.Diagnostics;

namespace EssentialLayers.Request.Helpers
{
	public sealed class GlobalFunctions
	{
		public static void Info(string url, string method, string json)
		{
			var info = new
			{
				url,
				method,
				json
			};

			Trace.WriteLine(info.Serialize(true));
		}

		public static void Error(Exception e)
		{
			Error(e.Message);
		}

		public static void Error(string message)
		{
			Trace.WriteLine($"Error: {message}");
		}
	}
}