using EssentialLayers.Request.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Services.Request
{
	public interface IRequestService
	{
		Task<HttpResponseMessage> DeleteAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponseMessage> GetAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponseMessage> PostAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		Task<HttpResponseMessage> PutAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		);

		void SetOptions(HttpOption httpOption);
	}
}