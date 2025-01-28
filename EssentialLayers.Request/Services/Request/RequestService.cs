using EssentialLayers.Request.Helpers.Request;
using EssentialLayers.Request.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace EssentialLayers.Request.Services.Request
{
	internal class RequestService : IRequestService
	{
		private readonly RequestHelper RequestHelper = new();

		/**/

		public async Task<HttpResponseMessage> DeleteAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await RequestHelper.SendAsync(
				request, url, HttpMethod.Delete, options
			);
		}

		public async Task<HttpResponseMessage> GetAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await RequestHelper.SendAsync(
				request, url, HttpMethod.Get, options
			);
		}

		public async Task<HttpResponseMessage> PostAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await RequestHelper.SendAsync(
				request, url, HttpMethod.Post, options
			);
		}

		public async Task<HttpResponseMessage> PutAsync<TRequest>(
			TRequest request, string url, RequestOptions? options = null
		)
		{
			return await RequestHelper.SendAsync(
				request, url, HttpMethod.Put, options
			);
		}

		public void SetOptions(HttpOption httpOption) => RequestHelper.SetOptions(httpOption);
	}
}