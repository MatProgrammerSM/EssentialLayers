namespace EssentialLayers.Helpers.Extension
{
	public static class DecimalExtension
	{
		public static string ToDecimalCurrency(this decimal value, bool bValidateZeroByEmpty = false)
		{
			if (bValidateZeroByEmpty) return value.Equals(0) ? string.Empty : $"{value:C2}";

			return $"{value:C2}";
		}
	}
}