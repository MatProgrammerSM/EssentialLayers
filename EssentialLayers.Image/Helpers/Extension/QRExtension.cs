using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ECCLevel = QRCoder.QRCodeGenerator.ECCLevel;

namespace EssentialLayers.Images.Helpers.Extension
{
	public static class QRExtension
	{
		public static byte[] ToBytes(
			this string self, int pixelsPerModule = 2, ECCLevel correctionLevel = ECCLevel.M
		)
		{
			using QRCodeGenerator qrGenerator = new();
			using QRCodeData qrCodeData = qrGenerator.CreateQrCode(self, correctionLevel);
			using PngByteQRCode qRCode = new(qrCodeData);

			return qRCode.GetGraphic(pixelsPerModule);
		}

		public static Bitmap ToBarCodeBitMap(
			this string text, Size size, ImageFormat format
		)
		{
			Image image = ToImage(text, format);

			return new Bitmap(image, size);
		}

		public static Image ToImage(
			this string self, ImageFormat format, int pixelsPerModule = 2, ECCLevel correctionLevel = ECCLevel.M
		)
		{
			byte[] bytes = ToBytes(self, pixelsPerModule, correctionLevel);

			using MemoryStream memoryStream = new(bytes);
			using (Bitmap bitmap = new(memoryStream))
			{
				bitmap.Save(memoryStream, format);
			}

			return Image.FromStream(memoryStream);
		}

		public static Stream ToStream(
			this string self, int pixelsPerModule = 2, ECCLevel correctionLevel = ECCLevel.M
		)
		{
			byte[] bytes = ToBytes(self, pixelsPerModule, correctionLevel);

			return new MemoryStream(bytes);
		}

		public static string ToBase64(
			this string self, int pixelsPerModule = 2, ECCLevel correctionLevel = ECCLevel.M
		)
		{
			byte[] bytes = ToBytes(self, pixelsPerModule, correctionLevel);

			return Convert.ToBase64String(bytes);
		}
	}
}