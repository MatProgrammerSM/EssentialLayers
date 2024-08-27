using System.Collections.Generic;
using System.Threading;

namespace EssentialLayers.Helpers.Http
{
    public class RequestOptions(bool isCached = false)
    {
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public bool IsCached { get; set; } = isCached;

        public CancellationToken CancellationToken { get; set; } = default;

        public string BearerToken { get; set; } = string.Empty;

        public string AppName { get; set; } = "DefaultApp";

        public string AppVersion { get; set; } = "1.0";
    }
}