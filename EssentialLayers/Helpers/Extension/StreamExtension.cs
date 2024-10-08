﻿using System;
using System.IO;

namespace EssentialLayers.Helpers.Extension
{
	public static class StreamExtension
	{
		public static byte[] ToBytes(this Stream stream)
		{
			using MemoryStream memoryStream = new();

			stream.CopyTo(memoryStream);

			return memoryStream.ToArray();
		}

		public static MemoryStream ToMemoryStream(this Stream stream)
		{
			using MemoryStream memoryStream = new();

			stream.CopyTo(memoryStream);

			return memoryStream;
		}

		public static void WriteFile(
			this Stream stream, string fullpath, string filename, Extension extension
		)
		{
			try
			{
				using MemoryStream memoryStream = new();
				stream.CopyTo(memoryStream);

				string path = $"{fullpath}{filename}.{extension.GetType().Name.ToLower()}";

				File.WriteAllBytes(path, memoryStream.ToArray());
			}
			catch (Exception e)
			{
				throw new Exception(e.Message, e);
			}
		}

		public enum Extension
		{
			TXT,
			PDF,
			XLSX,
			PNG,
			JPG,
			JSON,
			XML
		}
	}
}