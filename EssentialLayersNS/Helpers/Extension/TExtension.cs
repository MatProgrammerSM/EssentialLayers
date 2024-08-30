using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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

		public static DynamicParameters ParseDynamic<T>(this T self)
		{
			if (self == null) return new DynamicParameters();

			List<PropertyInfo> properties = [.. self.GetType().GetProperties()];

			DynamicParameters parameters = new();

			properties.ForEach(
				property =>
				{
					object value = property.GetValue(self);
					string fullName = property.PropertyType.FullName;

					DbType? dbType = DbTypeToCSharpTypes[fullName!];

					parameters.Add(property.Name, value, dbType);
				}
			);

			return parameters;
		}

		private static readonly Dictionary<string, DbType> DbTypeToCSharpTypes = new()
		{
			{"System.Byte[]", DbType.Binary},
			{"System.Byte", DbType.Byte},
			{"System.Boolean", DbType.Boolean},
			{"System.DateTime", DbType.DateTime},
			{"System.Decimal", DbType.Decimal},
			{"System.Double", DbType.Double},
			{"System.Guid", DbType.Guid},
			{"System.Int16", DbType.Int16},
			{"System.Int32", DbType.Int32},
			{"System.Int64", DbType.Int64},
			{"System.Object", DbType.Object},
			{"System.SByte", DbType.SByte},
			{"System.Single", DbType.Single},
			{"System.String", DbType.String},
			{"System.TimeSpan", DbType.Time},
			{"System.UInt16", DbType.UInt16},
			{"System.UInt32", DbType.UInt32},
			{"System.UInt64", DbType.UInt64},
			{"System.DateTimeOffset", DbType.DateTimeOffset}
		};

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