using System.Globalization;
using System.Linq;

namespace EssentialLayers.Helpers.Extension
{
	public static class IntExtensions
	{
		public static bool IsAny(this int self, params int[] values)
		{
			return values.Any(value => value == self);
		}

		public static bool Between(this int self, int start, int end)
		{
			return self >= start && self <= end;
		}

		public static bool NotBetween(this int self, int start, int end)
		{
			return self < start && self > end;
		}

		public static bool NotAny(this int self, params int[] values)
		{
			return values.All(value => self != value);
		}

		public static bool AreAll(this int self, params int[] values)
		{
			return values.All(value => self == value);
		}

		public static bool LessThanZero(this int self)
		{
			return self < 0;
		}

		public static string GetMonth(this int self)
		{
			string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames;

			return months[self is > 0 and < 13 ? self - 1 : 0].ToUpper();
		}

		public static string GetDay(this int self)
		{
			string[] days = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;

			return days[self is > 0 and < 8 ? self - 1 : 0];
		}

		public static bool InRange(this int self, int start, int end)
		{
			return Enumerable.Range(start, end).Contains(self);
		}

		public static bool NotInRange(this int self, int start, int end)
		{
			return Enumerable.Range(start, end).Contains(self).False();
		}

		public static int IndexOfRange(this int self, int[] values)
		{
			int start = 0;

			for (int i = 0; i < values.Length; i++)
			{
				int end = values[i];

				if (self >= start && self <= end) return i;

				start = values[i];
			}

			return -1;
		}
	}
}