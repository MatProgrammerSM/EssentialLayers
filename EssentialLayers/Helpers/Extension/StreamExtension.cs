using System;
using System.IO;
using System.IO.Compression;

namespace EssentialLayers.Helpers.Extension
{
	public static class StreamExtension
	{
		public static Stream Compress(
			this Stream inputStream, string fileName
		)
		{
			MemoryStream compressedStream = new();

			using (ZipArchive zipArchive = new(compressedStream, ZipArchiveMode.Create, leaveOpen: true))
			{
				ZipArchiveEntry zipEntry = zipArchive.CreateEntry(fileName, CompressionLevel.Optimal);

				using Stream destinationStream = zipEntry.Open();

				inputStream.CopyTo(destinationStream);
			}

			compressedStream.Seek(0, SeekOrigin.Begin);

			return compressedStream;
		}

		public static Stream Decompress(
			this Stream compressedStream
		)
		{
			MemoryStream decompressedStream = new();

			using (ZipArchive zipArchive = new(compressedStream, ZipArchiveMode.Read))
			{
				ZipArchiveEntry entry = zipArchive.Entries[0];

				using Stream entryStream = entry.Open();

				entryStream.CopyTo(decompressedStream);
			}

			decompressedStream.Seek(0, SeekOrigin.Begin);

			return decompressedStream;
		}

		public static byte[] ToBytes(
			this Stream stream
		)
		{
			using MemoryStream memoryStream = new();

			stream.CopyTo(memoryStream);

			return memoryStream.ToArray();
		}

		public static MemoryStream ToMemoryStream(
			this Stream stream
		)
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