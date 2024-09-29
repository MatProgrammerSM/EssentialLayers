using Dapper;
using EssentialLayers.Dapper.Extensions;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EssentialLayers.Dapper.Services.Database
{
	internal class DatabaseService : IDatabaseService
	{
		private string ConnectionString = string.Empty;

		/**/

		public void SetConnectionString(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public ResultHelper<TResult> Execute<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<TResult> result = ValidateConnectionString(
				Activator.CreateInstance<TResult>(), ConnectionString
			);

			if (result.Ok.False()) return result;

			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamicParameters();

				TResult query = sqlConnection.QueryFirst<TResult>(
					storedProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				result = ResultHelper<TResult>.Success(query);
			}
			catch (Exception e)
			{
				result = ResultHelper<TResult>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<TResult>> ExecuteAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			await using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<TResult> result = ValidateConnectionString(
				Activator.CreateInstance<TResult>(), ConnectionString
			);

			if (result.Ok.False()) return result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamicParameters();

				TResult query = await sqlConnection.QueryFirstAsync<TResult>(
					storedProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				result = ResultHelper<TResult>.Success(query);
			}
			catch (Exception e)
			{
				result = ResultHelper<TResult>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public ResultHelper<IEnumerable<TResult>> ExecuteAll<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<IEnumerable<TResult>> result = ValidateConnectionString<IEnumerable<TResult>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamicParameters();

				IEnumerable<TResult> query = sqlConnection.Query<TResult>(
					storedProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				result = ResultHelper<IEnumerable<TResult>>.Success(query);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<TResult>>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<IEnumerable<TResult>>> ExecuteAllAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			await using SqlConnection sqlConnection = new(ConnectionString);

			ResultHelper<IEnumerable<TResult>> result = ValidateConnectionString<IEnumerable<TResult>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			try
			{
				DynamicParameters dynamicParameters = request.ParseDynamicParameters();

				IEnumerable<TResult> query = await sqlConnection.QueryAsync<TResult>(
					storedProcedure, dynamicParameters, commandTimeout: 0,
					commandType: CommandType.StoredProcedure
				);

				result = ResultHelper<IEnumerable<TResult>>.Success(query);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<TResult>>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public ResultHelper<TResult> ExecuteComplex<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<TResult> result = ValidateConnectionString(
				Activator.CreateInstance<TResult>(), ConnectionString
			);

			if (result.Ok.False()) return result;

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storedProcedure, sqlConnection);

				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					TResult first = command.GetResults<TResult>().FirstOrDefault()!;

					result = ResultHelper<TResult>.Success(first);
				}
				catch (Exception e)
				{
					result = ResultHelper<TResult>.Fail(e);
				}
				finally
				{
					sqlConnection.Close();
					SqlConnection.ClearPool(sqlConnection);
				}
			}

			return result;
		}

		public async Task<ResultHelper<TResult>> ExecuteComplexAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<TResult> result = ValidateConnectionString(
				Activator.CreateInstance<TResult>(), ConnectionString
			);

			if (result.Ok.False()) return result;

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storedProcedure, sqlConnection);

				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					TResult first = (await command.GetResultsAsync<TResult>()).FirstOrDefault()!;

					result = ResultHelper<TResult>.Success(first);
				}
				catch (Exception e)
				{
					result = ResultHelper<TResult>.Fail(e);
				}
				finally
				{
					sqlConnection.Close();
					SqlConnection.ClearPool(sqlConnection);
				}
			}

			return result;
		}

		public ResultHelper<IEnumerable<TResult>> ExecuteComplexAll<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ValidateConnectionString<IEnumerable<TResult>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storedProcedure, sqlConnection);

				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					IEnumerable<TResult> results = command.GetResults<TResult>();

					result = ResultHelper<IEnumerable<TResult>>.Success(results);
				}
				catch (Exception e)
				{
					result = ResultHelper<IEnumerable<TResult>>.Fail(e);
				}
				finally
				{
					sqlConnection.Close();
					SqlConnection.ClearPool(sqlConnection);
				}
			}

			return result;
		}

		public async Task<ResultHelper<IEnumerable<TResult>>> ExecuteComplexAllAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ValidateConnectionString<IEnumerable<TResult>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storedProcedure, sqlConnection);

				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					IEnumerable<TResult> results = await command.GetResultsAsync<TResult>();

					result = ResultHelper<IEnumerable<TResult>>.Success(results);
				}
				catch (Exception e)
				{
					throw e;
				}
				finally
				{
					sqlConnection.Close();
					SqlConnection.ClearPool(sqlConnection);
				}
			}

			return result;
		}

		public ResultHelper<IEnumerable<IEnumerable<dynamic>>> ExecuteMultiple<TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ValidateConnectionString<IEnumerable<IEnumerable<dynamic>>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			DynamicParameters dynamicParameters = request.ParseDynamicParameters();

			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				GridReader gridReader = sqlConnection.QueryMultiple(
					storedProcedure, dynamicParameters, commandTimeout: 0, commandType: CommandType.StoredProcedure
				);

				while (!gridReader.IsConsumed)
				{
					resultSets.Add(gridReader.Read());
				}

				result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success(resultSets);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		public async Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> ExecuteMultipleAsync<TRequest>(
			TRequest request, string storedProcedure
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ValidateConnectionString<IEnumerable<IEnumerable<dynamic>>>(
				[], ConnectionString
			);

			if (result.Ok.False()) return result;

			DynamicParameters dynamicParameters = request.ParseDynamicParameters();

			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				GridReader gridReader = await sqlConnection.QueryMultipleAsync(
					storedProcedure, dynamicParameters, commandTimeout: 0, commandType: CommandType.StoredProcedure
				);

				while (!gridReader.IsConsumed)
				{
					resultSets.Add(gridReader.Read());
				}

				result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success(resultSets);
			}
			catch (Exception e)
			{
				result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Fail(e);
			}
			finally
			{
				SqlConnection.ClearPool(sqlConnection);
			}

			return result;
		}

		private ResultHelper<TResult> ValidateConnectionString<TResult>(
			TResult result, string connectionString
		)
		{
			bool isEmpty = connectionString.IsEmpty();

			if (isEmpty) return ResultHelper<TResult>.Fail(
				"The connection string wasn't initilized yet"
			);

			return ResultHelper<TResult>.Success(result);
		}
	}
}