using System.Threading.Tasks;

namespace EssentialLayers.Services.Http
{
    public interface IHttpWebService
	{
		void SetOptions(HttpWebHelperOption httpWebHelperOption);

		Task<HttpWebResponse<TResult>> GetAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		);

		Task<HttpWebResponse<TResult>> PostAsync<TResult, TRequest>(
			TRequest request, string url, RequestOptions options = null
		);
	}
}