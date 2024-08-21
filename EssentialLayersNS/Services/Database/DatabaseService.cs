using Dapper;
using EssentialLayers.Helpers.Result;
using EssentialLayersNS.Helpers.Extension;
using EssentialLayersNS.Helpers.Result;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EssentialLayersNS.Services.Database
{
	public class DatabaseService(string connectionString) : IDatabaseService
	{
		private readonly string ConnectionString = connectionString;

		public ResultHelper<TResult> Get<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response
		{
			using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<TResult> result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamic();

				TResult query = sqlConnection.QueryFirst<TResult>(
					storeProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				bool isValid = query!.SearchProperty<bool>(nameof(Response.Ok));
				string message = query!.SearchProperty<string>(nameof(Response.Message));

				if (isValid) result = ResultHelper<TResult>.Success(query);
				else result = ResultHelper<TResult>.Fail(message);
			}
			catch (Exception e)
			{
				result = ResultHelper<TResult>.Fail(e.Message);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public ResultHelper<IEnumerable<TResult>> GetAll<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response
		{
			using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<IEnumerable<TResult>> result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamic();

				IEnumerable<TResult> query = sqlConnection.Query<TResult>(
					storeProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				bool isValid = query!.SearchProperty<bool>(nameof(Response.Ok));
				string message = query!.SearchProperty<string>(nameof(Response.Message));

				if (isValid) result = ResultHelper<IEnumerable<TResult>>.Success(query);
				else result = ResultHelper<IEnumerable<TResult>>.Fail(message);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<TResult>>.Fail(e.Message);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<IEnumerable<TResult>>> GetAllAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response
		{
			await using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<IEnumerable<TResult>> result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamic();

				IEnumerable<TResult> query = await sqlConnection.QueryAsync<TResult>(
					storeProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				bool isValid = query!.SearchProperty<bool>(nameof(Response.Ok));
				string message = query!.SearchProperty<string>(nameof(Response.Message));

				if (isValid) result = ResultHelper<IEnumerable<TResult>>.Success(query);
				else result = ResultHelper<IEnumerable<TResult>>.Fail(message);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<TResult>>.Fail(e.Message);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response
		{
			await using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<TResult> result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamic();

				TResult query = await sqlConnection.QueryFirstAsync<TResult>(
					storeProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				bool isValid = query!.SearchProperty<bool>(nameof(Response.Ok));
				string message = query!.SearchProperty<string>(nameof(Response.Message));

				if (isValid) result = ResultHelper<TResult>.Success(query);
				else result = ResultHelper<TResult>.Fail(message);
			}
			catch (Exception e)
			{
				result = ResultHelper<TResult>.Fail(e.Message);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<TResult>> GetCombinedAsync<TResult, TRequest>(
			string storedProcedure, TRequest request
		) where TResult : Response
		{
			TResult respuesta = Activator.CreateInstance<TResult>();

			await using (SqlConnection connection = new(ConnectionString))
			{
				await using SqlCommand command = new(storedProcedure, connection);

				foreach (PropertyInfo prop in request!.GetType().GetProperties())
				{
					Tuple<object, SqlDbType> pair = AnalyzePropertyForList(
						prop.PropertyType, prop.GetValue(request)!
					);

					if (pair.Item2 != null && pair.Item2 == SqlDbType.Structured)
					{
						IEnumerable<object> list = pair.Item1 as IEnumerable<object>;

						if (list is not null)
						{
							using DataTable dataTable = new();
							object first = list.FirstOrDefault();

							if (first != null)
							{
								IDictionary<string, object> headers = first.ToDictionary<object>();

								foreach (KeyValuePair<string, object> header in headers)
								{
									dataTable.Columns.Add(header.Key);
								}

								foreach (object item in list)
								{
									IDictionary<string, object> dictionaries = item.ToDictionary<object>();
									DataRow row = dataTable.NewRow();

									foreach (KeyValuePair<string, object> dictionary in dictionaries)
									{
										row[dictionary.Key] = dictionary.Value;
									}

									dataTable.Rows.Add(row);
								}

								SqlParameter tvpParam = command.Parameters.AddWithValue(
									$"@{prop.Name}", dataTable
								);
							}
						}
						else
						{
							using DataTable dataTable = new();
							IDictionary<string, object> dictionaries = pair.Item1.ToDictionary<object>();
							DataRow row = dataTable.NewRow();

							foreach (KeyValuePair<string, object> item in dictionaries)
							{
								dataTable.Columns.Add(item.Key);
								row[item.Key] = item.Value;
							}

							dataTable.Rows.Add(row);

							SqlParameter tvpParam = command.Parameters.AddWithValue(
								$"@{prop.Name}", dataTable
							);
						}
					}
					else
					{
						SqlParameter tvpParam = command.Parameters.AddWithValue(
							$"@{prop.Name}", pair.Item1
						);
						tvpParam.SqlDbType = pair.Item2;
					}
				}

				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 3600;

				try
				{
					await connection.OpenAsync();

					if (connection.State == ConnectionState.Open)
					{
						await using SqlDataReader executeReader = await command.ExecuteReaderAsync(
							CommandBehavior.SingleRow
						);

						while (await executeReader.ReadAsync())
						{
							TResult tmpModel = Activator.CreateInstance<TResult>();

							ReadOnlyCollection<DbColumn> columns = executeReader.GetColumnSchema();

							foreach (DbColumn column in columns)
							{
								object value = executeReader.GetValue(column.ColumnName);

								if (value is not DBNull)
								{
									tmpModel.GetType().GetProperty(column.ColumnName)?.SetValue(tmpModel, value);
								}
							}

							respuesta = tmpModel;
						}
					}
				}
				catch (Exception e)
				{
					return await Task.FromResult(ResultHelper<TResult>.Fail($"DPGA {e.Message}"));
				}
				finally
				{
					await connection.CloseAsync();

					connection.Dispose();
					command.Dispose();
				}
			}

			return await Task.FromResult(ResultHelper<TResult>.Success(respuesta));
		}

		public async Task<ResultHelper<IList<TResult>>> GetAllCombinedAsync<TResult, TRequest>(
			string storedProcedure, TRequest request
		) where TResult : Response
		{
			IList<TResult> respuesta = [];

			await using (SqlConnection connection = new(ConnectionString))
			{
				await using SqlCommand command = new(storedProcedure, connection);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 3600;

				try
				{
					await connection.OpenAsync();

					if (connection.State == ConnectionState.Open)
					{
						await using SqlDataReader executeReader = await command.ExecuteReaderAsync();

						while (await executeReader.ReadAsync())
						{
							TResult tmpModel = Activator.CreateInstance<TResult>();
							ReadOnlyCollection<DbColumn> columns = executeReader.GetColumnSchema();

							foreach (DbColumn column in columns)
							{
								object value = executeReader.GetValue(column.ColumnName);

								if (value is not DBNull)
								{
									tmpModel.GetType().GetProperty(column.ColumnName)?.SetValue(tmpModel, value);
								}
							}

							respuesta.Add(tmpModel);
						}
					}
				}
				catch (Exception e)
				{
					return await Task.FromResult(ResultHelper<IList<TResult>>.Fail($"DPGA {e.Message}"));
				}
				finally
				{
					await connection.CloseAsync();

					connection.Dispose();
					command.Dispose();
				}
			}

			return await Task.FromResult(ResultHelper<IList<TResult>>.Success(respuesta));
		}

		private static Tuple<object, SqlDbType> AnalyzePropertyForList(Type type, object value)
		{
			SqlDbType dbType = SqlDbType.Variant;

			switch (type.FullName)
			{
				case "System.Boolean":

					dbType = SqlDbType.Bit;

					break;

				case "System.Byte":

					dbType = SqlDbType.Bit;

					break;

				case "System.Byte[]":

					dbType = SqlDbType.Binary;

					break;

				case "System.Int32":

					dbType = SqlDbType.Int;

					break;

				case "System.String":

					dbType = SqlDbType.VarChar;
					value ??= string.Empty;

					break;

				case "System.DateTime":

					dbType = SqlDbType.DateTime;
					value ??= DateTimeExtension.DEFAULT;

					break;

				case "System.Decimal":

					dbType = SqlDbType.Decimal;

					break;

				case "System.Single":

					dbType = SqlDbType.Float;

					break;

				case "System.Int64":

					dbType = SqlDbType.BigInt;

					break;

				default:

					dbType = SqlDbType.Structured;

					break;
			}

			return new Tuple<object, SqlDbType>(value, dbType);
		}
	}
}