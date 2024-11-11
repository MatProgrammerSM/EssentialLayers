namespace EssentialLayers.Helpers.Extension
{
	public static class LongExtension
	{
		public static bool LessThanZero(
			this long self
		)
		{
			return self < 0;
		}

		public static long EqualsLessThanZero(
			this long self, long def = -1
		)
		{
			return self.LessThanZero() ? def : self;
		}
	}
}