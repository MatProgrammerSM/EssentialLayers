using System;
using System.Collections.Generic;
using System.Reflection;

namespace EssentialLayersNS.Helpers.Extension
{
	public static class ObjectExtensions
	{
		public static IDictionary<string, TValue> ToDictionary<TValue>(this object self)
		{
			PropertyInfo[] properties = self.GetType().GetProperties();
			IDictionary<string, TValue> result = new Dictionary<string, TValue>();

			foreach (PropertyInfo property in properties)
			{
				string key = property.Name;
				TValue? value = (TValue)property.GetValue(self, null)!;

				result.Add(new KeyValuePair<string, TValue>(key, value));
			}

			return result;
		}

		public static int ToInt(this object self)
		{
			if (int.TryParse(self?.ToString(), out int value)) return value;

			return default;
		}

		public static long ToLong(this object self)
		{
			if (long.TryParse(self?.ToString(), out long value)) return value;

			return default;
		}

		public static ulong ToULong(this object self)
		{
			if (ulong.TryParse(self?.ToString(), out ulong value)) return value;

			return default;
		}

		public static DateTime ToDateTime(this object self)
		{
			if (DateTime.TryParse(self?.ToString(), out DateTime value)) return value;

			return default;
		}

		public static decimal ToDecimal(this object self)
		{
			if (decimal.TryParse(self?.ToString(), out decimal value)) return value;

			return default;
		}

		public static double ToDouble(this object self)
		{
			if (double.TryParse(self?.ToString(), out double value)) return value;

			return default;
		}

		public static float ToFloat(this object self)
		{
			if (float.TryParse(self?.ToString(), out float value)) return value;

			return default;
		}

		public static short ToShort(this object self)
		{
			if (short.TryParse(self?.ToString(), out var value)) return value;

			return default;
		}

		public static byte ToByte(this object self)
		{
			if (byte.TryParse(self?.ToString(), out byte value)) return value;

			return default;
		}

		public static bool ToBoolean(this object self)
		{
			if (bool.TryParse(self.ToString(), out bool value)) return value;

			return default;
		}

		public static bool IsNull(this object self)
		{
			return self == null;
		}

		public static bool NotNull(this object self)
		{
			return self != null;
		}

		public static bool NotEquals(this object self, object other)
		{
			return !self.Equals(other);
		}
	}
}