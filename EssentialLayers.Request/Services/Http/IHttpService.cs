using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Models;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Services.Http
{
    public interface IHttpService
	{
		Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponse<TResult>> PutAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		void SetOptions(HttpOption httpOption);
	}
}