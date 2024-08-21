using EssentialLayers.Helpers.Result;
using EssentialLayers.Helpers.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayers.Services.Database
{
	public interface IDatabaseService
	{
		ResultHelper<TResult> Get<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response;

		Task<ResultHelper<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response;

		ResultHelper<IEnumerable<TResult>> GetAll<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response;

		Task<ResultHelper<IEnumerable<TResult>>> GetAllAsync<TResult, TRequest>(
			TRequest request, string storeProcedure
		) where TResult : Response;

		Task<ResultHelper<TResult>> GetCombinedAsync<TResult, TRequest>(
			string storedProcedure, TRequest request
		) where TResult : Response;

		Task<ResultHelper<IList<TResult>>> GetAllCombinedAsync<TResult, TRequest>(
			string storedProcedure, TRequest request
		) where TResult : Response;
	}
}