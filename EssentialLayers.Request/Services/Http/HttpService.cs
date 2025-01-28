using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Helpers.Http;
using EssentialLayers.Request.Models;
using System.Net.Http;
using System.Threading.Tasks;
using RequestOptions = EssentialLayers.Request.Models.RequestOptions;

namespace EssentialLayers.Request.Services.Http
{
	internal class HttpService : IHttpService
	{
		private readonly HttpHelper HttpHelper = new();

		/**/

		public async Task<HttpResponse<TResult>> DeleteAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await HttpHelper.SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Delete, options
			);
		}

		public async Task<HttpResponse<TResult>> GetAsync<TResult>(
			string url, RequestOptions? options = null
		)
		{
			return await HttpHelper.SendAsync<TResult, object>(
				null, url, HttpMethod.Get, options
			);
		}

		public async Task<HttpResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await HttpHelper.SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Post, options
			);
		}

		public async Task<HttpResponse<TResult>> PutAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await HttpHelper.SendAsync<TResult, TRequest>(
				request, url, HttpMethod.Put, options
			);
		}

		public void SetOptions(HttpOption httpOption) => HttpHelper.SetOptions(httpOption);
	}
}