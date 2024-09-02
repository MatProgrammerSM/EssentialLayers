using System.Linq;

namespace EssentialLayers.Helpers.Extension
{
	public static class LongExtension
	{
		public static bool IsAny(
			this long value, params long[] values
		)
		{
			return values.Count(val => val == value) > 0;
		}

		public static bool NotAny(
			this long value, params long[] values
		)
		{
			return values.All(val => value != val);
		}

		public static bool LessThanZero(
			this long i
		)
		{
			return i < 0;
		}

		public static long EqualsLessThanZero(
			this long self, long def = -1
		)
		{
			return self.LessThanZero() ? def : self;
		}
	}
}