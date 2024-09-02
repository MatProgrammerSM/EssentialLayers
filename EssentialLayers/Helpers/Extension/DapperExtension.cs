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
			{ "Byte[]", DbType.Binary },
			{ "Byte", DbType.Byte },
			{ "Boolean", DbType.Boolean },
			{ "DateTime", DbType.DateTime },
			{ "Decimal", DbType.Decimal },
			{ "Double", DbType.Double },
			{ "Guid", DbType.Guid },
			{ "Int16", DbType.Int16 },
			{ "Int32", DbType.Int32 },
			{ "Int64", DbType.Int64 },
			{ "Object", DbType.Object },
			{ "IList`1", DbType.Object },
			{ "List`1", DbType.Object },
			{ "SByte", DbType.SByte },
			{ "Single", DbType.Single },
			{ "String", DbType.String },
			{ "TimeSpan", DbType.Time },
			{ "UInt16", DbType.UInt16 },
			{ "UInt32", DbType.UInt32 },
			{ "UInt64", DbType.UInt64 },
			{ "DateTimeOffset", DbType.DateTimeOffset }
		};

		private static readonly Dictionary<string, SqlDbType> SqlDbTypeToCSharpTypes = new()
		{
			{ "Int64", SqlDbType.BigInt },
			{ "Boolean", SqlDbType.Bit },
			{ "Char", SqlDbType.Char },
			{ "DateTime", SqlDbType.DateTime },
			{ "Decimal", SqlDbType.Decimal },
			{ "Double", SqlDbType.Float },
			{ "Int32", SqlDbType.Int },
			{ "Single", SqlDbType.Real },
			{ "Guid", SqlDbType.UniqueIdentifier },
			{ "Int16", SqlDbType.SmallInt },
			{ "Byte", SqlDbType.TinyInt },
			{ "Byte[]", SqlDbType.VarBinary },
			{ "String", SqlDbType.VarChar },
			{ "Object", SqlDbType.Structured },
			{ "IList`1", SqlDbType.Structured },
			{ "List`1", SqlDbType.Structured },
			{ "TimeSpan", SqlDbType.Time },
			{ "DateTimeOffset", SqlDbType.DateTimeOffset }
		};

		public static SqlDbType GetSqlDbType(this Type type)
		{
			if (type == null) return SqlDbType.Structured;

			SqlDbType result = SqlDbTypeToCSharpTypes[type.Name!];

			return result;
		}

		public static DbType GetDbType(this Type type)
		{
			if (type == null) return DbType.Object;

			DbType result = DbTypeToCSharpTypes[type.Name];

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
					object value = property.GetValue(self)!;
					string name = property.PropertyType.Name;

					DbType dbType = DbTypeToCSharpTypes[name];

					string parameterName = $"@{property.Name}";

					if (dbType == DbType.Object)
					{
						IEnumerable<object> enumerable = (value as IEnumerable<object>)!;
						DataTable dataTable = enumerable.ParseDataTable();
						string typeName = enumerable.FirstOrDefault()!.GetType().Name;

						dinamicParameters.Add(parameterName, dataTable);
					}
					else
					{
						dinamicParameters.Add(parameterName, value, dbType);
					}
				}
			);

			return dinamicParameters;
		}

		public static DataTable ParseDataTable<T>(this IEnumerable<T> selfs)
		{
			if (selfs == null) return new DataTable();

			using DataTable result = new();
			PropertyInfo[] propertiesFirst = selfs.FirstOrDefault()!.GetType().GetProperties();

			foreach (PropertyInfo property in propertiesFirst)
			{
				result.Columns.Add(property.Name);
			}

			foreach (T self in selfs)
			{
				DataRow dataRow = result.NewRow();
				PropertyInfo[] properties = self!.GetType().GetProperties();

				foreach (PropertyInfo propertyInfo in properties)
				{
					string propertyName = propertyInfo.Name;
					object propertyValue = propertyInfo.GetValue(self)!;

					dataRow[propertyName] = propertyValue;
				}

				result.Rows.Add(dataRow);
			}

			return result;
		}
	}
}