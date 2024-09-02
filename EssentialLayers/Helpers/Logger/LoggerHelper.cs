using System;

namespace EssentialLayers.Helpers.Logger
{
	public static class LoggerHelper
	{
		public static void Info(string message)
		{
			Debug(CategoryAttribute.Info, message);
		}

		public static void Error(Exception e, string message)
		{
			Debug(CategoryAttribute.Error, $"\r\nMessage: {message}\r\n{e}");
		}

		public static void Warning(string message)
		{
			Debug(CategoryAttribute.Warning, message);
		}

		private static void Debug(CategoryAttribute categoryAttribute, string message)
		{
			string category = GetCategory(categoryAttribute);

			System.Diagnostics.Debug.WriteLine(message, category);
		}

		private static string GetCategory(CategoryAttribute categoryAttribute)
		{
			return categoryAttribute switch
			{
				CategoryAttribute.Info => " -> Info ",
				CategoryAttribute.Error => " -> Error ",
				CategoryAttribute.Warning => " -> Warning ",
				_ => string.Empty,
			};
		}

		private enum CategoryAttribute
		{
			Error,
			Info,
			Warning
		}
	}
}