using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EssentialLayers.Helpers.Extension
{
	public static class TExtension
	{
		public static bool IsAny<T>(
			this T self, IEnumerable<T> values
		)
		{
			if (self == null) return false;

			return values.Any(x => EqualityComparer<T>.Default.Equals(x, self));
		}

		public static bool NotAny<T>(
			this T self, IEnumerable<T> values
		)
		{
			return self.IsAny(values).False();
		}

		public static bool IsAny<T>(
			this T self, params T[] values
		)
		{
			return self.IsAny(values);
		}

		public static bool NotAny<T>(
			this T self, params T[] values
		)
		{
			return self.IsAny(values).False();
		}

		public static T DeepCopy<T>(this T self)
		{
			try
			{
				string serialized = JsonSerializer.Serialize(self);

				return JsonSerializer.Deserialize<T>(serialized)!;
			}
			catch (Exception e)
			{
				throw new Exception("Error on make the deep copy from the object", e);
			}
		}

		public static string Serialize<T>(
			this T self, bool indented = false, bool insensitive = false
		)
		{
			try
			{
				return JsonSerializer.Serialize(
					self, new JsonSerializerOptions
					{
						WriteIndented = indented,
						PropertyNameCaseInsensitive = insensitive
					}
				);
			}
			catch (Exception e)
			{
				throw new Exception("Error on serialize object", e);
			}
		}

		public static T Deserialize<T>(
			this string self, bool indented = false, bool insensitive = false
		)
		{
			try
			{
				return JsonSerializer.Deserialize<T>(
					self, new JsonSerializerOptions
					{
						WriteIndented = indented,
						PropertyNameCaseInsensitive = insensitive
					}
				)!;
			}
			catch (Exception e)
			{
				throw new Exception("Error on deserialize object", e);
			}
		}

		public static bool IsSimpleType<T>(this T self)
		{
			if (self is null)
			{
				Type type = self!.GetType();

				return type.IsPrimitive ||
					new Type[] {
					typeof(int),
					typeof(double),
					typeof(string),
					typeof(decimal),
					typeof(DateTime),
					typeof(DateTimeOffset),
					typeof(TimeSpan),
					typeof(Guid)
				}.Contains(type);
			}

			return false;
		}

		public static T SearchProperty<T>(this object obj, params string[] propertyNames)
		{
			try
			{
				PropertyInfo[] properties = obj.GetType().GetProperties();

				foreach (string propertyName in propertyNames)
				{
					PropertyInfo property = properties.FirstOrDefault(p => p.Name == propertyName);

					if (!string.IsNullOrEmpty(propertyName)) return (T)property!.GetValue(obj)!;
				}
			}
			catch (Exception e)
			{
				throw new Exception("The property doesn't exists in the object", e);
			}

			return default!;
		}

		public static bool NotEquals<T, K>(this T self, K other)
		{
			string serializedSelf = self.Serialize();
			string serializedOther = other.Serialize();

			return !serializedSelf.Equals(serializedOther);
		}

		public static bool AreEquals<T, K>(this T self, K other)
		{
			string serializedSelf = self.Serialize();
			string serializedOther = other.Serialize();

			return serializedSelf.Equals(serializedOther);
		}
	}
}