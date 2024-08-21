using EssentialLayers.Helpers.Result;
using System.Threading.Tasks;

namespace EssentialLayersNS.Services.HttpService
{
	public interface IHttpService
	{
		Task<HttpWebResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		) where TResult : Response;

		Task<HttpWebResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		) where TResult : Response;
	}
}