using System.Collections.Generic;
using System.Threading;

namespace EssentialLayers.Services.Http
{
    public class RequestOptions(bool isCached = false)
    {
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public bool IsCached { get; set; } = isCached;

        public CancellationToken CancellationToken { get; set; } = default;

        public string BearerToken { get; set; } = string.Empty;
    }
}