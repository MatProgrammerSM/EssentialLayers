namespace EssentialLayers.Helpers.Extension
{
	public static class DateTimeExtension
	{
		public static readonly DateTime DEFAULT = new(1900, 1, 1);

		public static bool IsAny(this DateTime value, params DateTime[] values)
		{
			return values.Any(val => val == value);
		}

		public static bool NotAny(this DateTime value, params DateTime[] values)
		{
			return values.Any(val => val != value);
		}

		public static string ToShortFormatMX(this DateTime value, bool bWithSlash = true)
		{
			if (bWithSlash) return Convert.ToDateTime(
				value.ToString()
			).ToString("dd/MM/yyyy");

			return Convert.ToDateTime(value.ToString()).ToString("dd-MM-yyyy");
		}

		public static string ToFullFormatMX(this DateTime value, bool bWithSlash = true)
		{
			if (bWithSlash) return Convert.ToDateTime(
				value.ToString()
			).ToString("dd/MM/yyyy hh:mm:ss tt");

			return Convert.ToDateTime(value.ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
		}

		public static bool IsEqualsToDefault(this DateTime datetime, bool full = true)
		{
			DateTime date = datetime;

			if (!full) date = DateTime.Parse(datetime.ToShortDateString());

			int value = DateTime.Compare(DEFAULT, date);

			return value == 0;
		}

		public static string EqualsDefaultToEmpty(this DateTime datetime, string date)
		{
			int value = DateTime.Compare(DEFAULT, datetime);

			return value == 0 ? string.Empty : date;
		}
	}
}