using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EssentialLayers.Helpers.Extension
{
	public static class TExtension
	{
		private static readonly JsonSerializerOptions SerializerOptions = new()
		{
			PropertyNameCaseInsensitive = true
		};

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

		public static string Serialize<T>(this T self, bool ident = false)
		{
			try
			{
				return JsonSerializer.Serialize(
					self, new JsonSerializerOptions
					{
						WriteIndented = ident,
					}
				);
			}
			catch (Exception e)
			{
				throw new Exception("Error on serialize object", e);
			}
		}

		public static T Deserialize<T>(this string self)
		{
			return JsonSerializer.Deserialize<T>(self, SerializerOptions)!;
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