using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EssentialLayers.Helpers.Extension
{
	public static class DapperExtension
	{
		private static readonly Dictionary<string, DbType> DbTypeToCSharpTypes = new()
		{
			{ "System.Byte[]", DbType.Binary },
			{ "System.Byte", DbType.Byte },
			{ "System.Boolean", DbType.Boolean },
			{ "System.DateTime", DbType.DateTime },
			{ "System.Decimal", DbType.Decimal },
			{ "System.Double", DbType.Double },
			{ "System.Guid", DbType.Guid },
			{ "System.Int16", DbType.Int16 },
			{ "System.Int32", DbType.Int32 },
			{ "System.Int64", DbType.Int64 },
			{ "System.Object", DbType.Object },
			{ "System.SByte", DbType.SByte },
			{ "System.Single", DbType.Single },
			{ "System.String", DbType.String },
			{ "System.TimeSpan", DbType.Time },
			{ "System.UInt16", DbType.UInt16 },
			{ "System.UInt32", DbType.UInt32 },
			{ "System.UInt64", DbType.UInt64 },
			{ "System.DateTimeOffset", DbType.DateTimeOffset }
		};

		private static readonly Dictionary<string, SqlDbType> SqlDbTypeToCSharpTypes = new()
		{
			{ "System.Int64", SqlDbType.BigInt },
			{ "System.Boolean", SqlDbType.Bit },
			{ "System.Char", SqlDbType.Char },
			{ "System.DateTime", SqlDbType.DateTime },
			{ "System.Decimal", SqlDbType.Decimal },
			{ "System.Double", SqlDbType.Float },
			{ "System.Int32", SqlDbType.Int },
			{ "System.Single", SqlDbType.Real },
			{ "System.Guid", SqlDbType.UniqueIdentifier },
			{ "System.Int16", SqlDbType.SmallInt },
			{ "System.Byte", SqlDbType.TinyInt },
			{ "System.Byte[]", SqlDbType.VarBinary },
			{ "System.String", SqlDbType.VarChar },
			{ "System.Object", SqlDbType.Structured },
			{ "System.TimeSpan", SqlDbType.Time },
			{ "System.DateTimeOffset", SqlDbType.DateTimeOffset }
		};

		public static IDictionary<TValue, TKey> InverseDictionary<TKey, TValue>(
			this Dictionary<TKey, TValue> self
		)
		{
			if (self == null) return new Dictionary<TValue, TKey>();

			IDictionary<TValue, TKey> result = new Dictionary<TValue, TKey>();

			foreach (KeyValuePair<TKey, TValue> keyValuePair in self)
			{
				TKey key = keyValuePair.Key;
				TValue value = keyValuePair.Value;

				result.Add(value, key);
			}

			return result;
		}

		public static SqlDbType GetSqlDbType(this Type type)
		{
			if (type == null) return SqlDbType.Structured;

			SqlDbType result = SqlDbTypeToCSharpTypes[type.FullName];

			return result;
		}

		public static DbType GetDbType(this Type type)
		{
			if (type == null) return DbType.Object;

			DbType result = DbTypeToCSharpTypes[type.FullName];

			return result;
		}

		public static DynamicParameters ParseDynamicParameters<T>(this T self)
		{
			if (self == null) return new DynamicParameters();

			List<PropertyInfo> properties = [.. self.GetType().GetProperties()];
			DynamicParameters dinamicParameters = new();

			properties.ForEach(
				property =>
				{
					object value = property.GetValue(self);
					string fullName = property.PropertyType.FullName;

					DbType dbType = DbTypeToCSharpTypes[fullName];

					if (property.Name.NotEquals("StoredProcedure"))
					{
						string parameterName = $"@{property.Name}";

						if (dbType == DbType.Object)
						{
							IEnumerable<T> enumerable = (value as IEnumerable<T>).Cast<T>();
							DataTable dataTable = enumerable.ParseDataTable();
							string typeName = enumerable.FirstOrDefault().GetType().Name;

							dinamicParameters.Add(parameterName, dataTable, dbType);
						}
						else
						{
							dinamicParameters.Add(parameterName, value, dbType);
						}

						dinamicParameters.Add(property.Name, value, dbType);
					}
				}
			);

			return dinamicParameters;
		}

		public static DataTable ParseDataTable<T>(this IEnumerable<T> selfs)
		{
			if (selfs == null) return new DataTable();

			using DataTable result = new();
			PropertyInfo[] propertiesFirst = selfs.FirstOrDefault().GetType().GetProperties();

			foreach (PropertyInfo property in propertiesFirst)
			{
				result.Columns.Add(property.Name, property.GetType());
			}

			foreach (T self in selfs)
			{
				DataRow dataRow = result.NewRow();
				PropertyInfo[] properties = self.GetType().GetProperties();

				foreach (PropertyInfo propertyInfo in properties)
				{
					string propertyName = propertyInfo.Name;
					object propertyValue = propertyInfo.GetValue(self);

					dataRow[propertyName] = propertyValue;
				}

				result.Rows.Add(dataRow);
			}

			return result;
		}
	}
}