using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EssentialLayers.Dapper.Extensions
{
	public static class SqlCommandExtension
	{
		public static IEnumerable<T> GetResults<T>(
			this SqlCommand sqlCommand
		)
		{
			IList<T> result = [];

			using (SqlDataReader executeReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow))
			{
				while (executeReader.Read())
				{
					T instance = Activator.CreateInstance<T>();
					ReadOnlyCollection<DbColumn> columns = executeReader.GetColumnSchema();

					foreach (DbColumn column in columns)
					{
						object value = executeReader.GetValue(column.ColumnName);

						instance.GetType().GetProperty(column.ColumnName).SetValue(instance, value);
					}

					result.Add(instance);
				}
			}

			return result;
		}

		public static async Task<IEnumerable<T>> GetResultsAsync<T>(
			this SqlCommand sqlCommand
		)
		{
			IList<T> result = [];

			using (SqlDataReader executeReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow))
			{
				while (await executeReader.ReadAsync())
				{
					T instance = Activator.CreateInstance<T>();
					ReadOnlyCollection<DbColumn> columns = executeReader.GetColumnSchema();

					foreach (DbColumn column in columns)
					{
						object value = executeReader.GetValue(column.ColumnName);

						instance!.GetType().GetProperty(column.ColumnName)!.SetValue(instance, value);
					}

					result.Add(instance);
				}
			}

			return result;
		}

		public static SqlParameter[] ParseSqlParameters(this DynamicParameters dynamicParameters)
		{
			IList<SqlParameter> sqlParameters = [];

			foreach (string parameterName in dynamicParameters.ParameterNames)
			{
				object value = dynamicParameters.Get<object>(parameterName);

				SqlParameter sqlParameter = new()
				{
					ParameterName = parameterName,
					Value = value
				};

				sqlParameters.Add(sqlParameter);
			}

			return [.. sqlParameters];
		}

		public static SqlParameter[] ToSqlParameterCollection<T>(
			this T self
		)
		{
			List<PropertyInfo> properties = [.. self!.GetType().GetProperties()];
			SqlParameter[] sqlParameters = new SqlParameter[properties.Count];

			foreach (var property in properties)
			{
				DbType sqlDbType = property.PropertyType.GetDbType();
				object value = property.GetValue(self)!;
				string parameterName = $"@{property.Name}";

				SqlParameter sqlParameter = new()
				{
					ParameterName = parameterName,
					Value = value,
					DbType = sqlDbType
				};

				sqlParameters.Append(sqlParameter);
			}

			return sqlParameters;
		}
	}
}