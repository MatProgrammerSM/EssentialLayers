using System;
using System.Collections.Generic;
using System.Linq;

namespace EssentialLayers.Helpers.Extension
{
	public static class DecimalExtension
	{
		public static string ToDecimalCurrency(
			this decimal value, bool isZeroEmpty = false
		)
		{
			if (isZeroEmpty) return value == 0 ? string.Empty : $"{value:C2}";

			return $"{value:C2}";
		}

		public static decimal Trim(
			this decimal value, int decimales
		)
		{
			string[] split = value.ToString().Split('.');
			int indexOfDot = split[1].Length;

			if (indexOfDot > decimales)
			{
				split[1] = split[1][..decimales];
				value = Convert.ToDecimal($"{split[0]}.{split[1]}");
			}

			return value;
		}
	}
}