using Dapper;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Http;
using EssentialLayers.Helpers.Result;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EssentialLayers.Helpers.Database
{
	public class DatabaseHelper(string connectionString)
	{
		private static HttpWebHelper get;

		private readonly string ConnectionString = connectionString;

		/**/

		public static HttpWebHelper Get => get ??= new HttpWebHelper();

		/**/

		public ResultHelper<TResult> Execute<TResult, TRequest>(
			TRequest request, string storeProcedure
		)
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
			TRequest request, string storeProcedure
		)
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
			TRequest request, string storeProcedure
		)
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
			TRequest request, string storeProcedure
		)
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

		public ResultHelper<TResult> GetCombined<TResult, TRequest>(
			TRequest request, string storeProcedure
		)
		{
			ResultHelper<TResult> result = ResultHelper<TResult>.Success(Activator.CreateInstance<TResult>());

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);
				
				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					TResult first = command.GetResults<TResult>().FirstOrDefault();

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

		public async Task<ResultHelper<TResult>> GetCombinedAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		)
		{
			ResultHelper<TResult> result = ResultHelper<TResult>.Success(Activator.CreateInstance<TResult>());

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);

				DynamicParameters dynamicParameters = request.ParseDynamicParameters();
				SqlParameter[] sqlParameters = dynamicParameters.ParseSqlParameters();

				command.Parameters.AddRange(sqlParameters);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandTimeout = 0;

				try
				{
					sqlConnection.Open();

					TResult first = (await command.GetResultsAsync<TResult>()).FirstOrDefault();

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

		public ResultHelper<IEnumerable<TResult>> GetAllCombined<TResult, TRequest>(
			TRequest request, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ResultHelper<IEnumerable<TResult>>.Success([]);

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);

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

		public async Task<ResultHelper<IEnumerable<TResult>>> GetAllCombinedAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ResultHelper<IEnumerable<TResult>>.Success([]);

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);

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

		public ResultHelper<IEnumerable<IEnumerable<dynamic>>> QueryMultiple(
			string query
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success([]);

			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				using GridReader gridReader = sqlConnection.QueryMultiple(
					query, commandTimeout: 0, commandType: CommandType.Text
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

		public async Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> QueryMultipleAsync(
			string query
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success([]);
			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				using GridReader gridReader = await sqlConnection.QueryMultipleAsync(
					query, commandTimeout: 0, commandType: CommandType.Text
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

		public ResultHelper<IEnumerable<IEnumerable<dynamic>>> QueryMultiple(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success([]);
			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				GridReader gridReader = sqlConnection.QueryMultiple(
					storeProcedure, dynamicParameters, commandTimeout: 0, commandType: CommandType.StoredProcedure
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

		public async Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> QueryMultipleAsync(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<IEnumerable<dynamic>>> result = ResultHelper<IEnumerable<IEnumerable<dynamic>>>.Success([]);
			using SqlConnection sqlConnection = new(ConnectionString);

			try
			{
				List<IEnumerable<dynamic>> resultSets = [];

				GridReader gridReader = await sqlConnection.QueryMultipleAsync(
					storeProcedure, dynamicParameters, commandTimeout: 0, commandType: CommandType.StoredProcedure
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
	}
}