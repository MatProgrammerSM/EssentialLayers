using Dapper;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EssentialLayers.Helpers.Database
{
	public class DatabaseHelper(string connectionString)
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

		public ResultHelper<TResult> GetCombined<TResult>(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<TResult> result = ResultHelper<TResult>.Success(Activator.CreateInstance<TResult>());

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);
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

		public async Task<ResultHelper<TResult>> GetCombinedAsync<TResult>(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<TResult> result = ResultHelper<TResult>.Success(Activator.CreateInstance<TResult>());

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);
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

		public ResultHelper<IEnumerable<TResult>> GetAllCombined<TResult>(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ResultHelper<IEnumerable<TResult>>.Success([]);

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);
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

		public async Task<ResultHelper<IEnumerable<TResult>>> GetAllCombinedAsync<TResult>(
			DynamicParameters dynamicParameters, string storeProcedure
		)
		{
			ResultHelper<IEnumerable<TResult>> result = ResultHelper<IEnumerable<TResult>>.Success([]);

			using (SqlConnection sqlConnection = new(ConnectionString))
			{
				using SqlCommand command = new(storeProcedure, sqlConnection);
				
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

		public ResultHelper<IEnumerable<IEnumerable<dynamic>>> GetQueryMultiple(
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

		public async Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> GetQueryMultipleAsync(
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

		public ResultHelper<IEnumerable<IEnumerable<dynamic>>> GetQueryMultiple(
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

		public async Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> GetQueryMultipleAsync(
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