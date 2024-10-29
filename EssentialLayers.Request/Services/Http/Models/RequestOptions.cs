using System.Collections.Generic;
using System.Threading;

namespace EssentialLayers.Request.Services.Http.Models
{
	public class RequestOptions
	{
        public RequestOptions() { }

        public RequestOptions(IDictionary<string, string> headers)
        {
            Headers = headers;
        }

        public RequestOptions(bool isCached = false)
        {
			IsCached = isCached;
        }

		public RequestOptions(string bearerToken)
		{
			BearerToken = bearerToken;
		}

		public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

		public bool IsCached { get; set; }

		public CancellationToken CancellationToken { get; set; } = default;

		public string BearerToken { get; set; } = string.Empty;
	}
}