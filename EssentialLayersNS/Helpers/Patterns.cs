namespace EssentialLayers.Helpers
{
	public static class Patterns
	{
		public const string Letters = @"^[A-Za-z a-ñA-ÑÓóÚúÜü&Üü#/-_{}+;:.]*$";

		public const string Numbers = @"^[0-9 1-9]*$";

		public const string LettersAndNumbers = @"^[A-Za-z a-ñA-Ñ 0-9 1-9 -&]*$";

		public const string Address = @"^[A-Za-z a-ñA-ÑÓóÚúÜü&Üü#/-_{}+;:. 0-9 1-9]*$";

		public const string Colony = @"^[A-Za-z a-ñA-ÑÓóÚúÜü&°)(/-_{}+,;:. 0-9 1-9]*$";

		public const string ExtractData = @"""data"":\s*({[^{}]*})";
	}
}