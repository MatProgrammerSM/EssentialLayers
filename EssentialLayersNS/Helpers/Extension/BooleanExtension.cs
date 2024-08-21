namespace EssentialLayersNS.Helpers.Extension
{
	public static class BooleanExtension
	{
		public static bool False(this bool self)
		{
			return !self;
		}

		public static bool And(this bool self, bool b)
		{
			return self && b;
		}

		public static bool Or(this bool self, bool b)
		{
			return self || b;
		}

		public static bool Same(this bool self, bool b)
		{
			return self == b;
		}
	}
}