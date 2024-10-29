using EssentialLayers.Request.Services.Helpers;
using EssentialLayers.Request.Services.Http.Models;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Services.Http
{
	public interface IHttpService
	{
		void SetOptions(HttpOption httpOption);

		Task<HttpResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> PutAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);
	}
}