﻿using EssentialLayers.Helpers.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayers.Services.Database
{
	public interface IDatabaseService
	{
		void SetConnectionString(string connectionString);

		ResultHelper<TResult> Execute<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		Task<ResultHelper<TResult>> ExecuteAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		ResultHelper<IEnumerable<TResult>> ExecuteAll<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		Task<ResultHelper<IEnumerable<TResult>>> ExecuteAllAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		ResultHelper<TResult> ExecuteComplex<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		ResultHelper<IEnumerable<TResult>> ExecuteComplexAll<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		Task<ResultHelper<IEnumerable<TResult>>> ExecuteComplexAllAsync<TResult, TRequest>(
			TRequest request, string storedProcedure
		);

		ResultHelper<IEnumerable<IEnumerable<dynamic>>> QueryMultiple<TRequest>(
			TRequest request, string storedProcedure
		);

		Task<ResultHelper<IEnumerable<IEnumerable<dynamic>>>> QueryMultipleAsync<TRequest>(
			TRequest request, string storedProcedure
		);
	}
}