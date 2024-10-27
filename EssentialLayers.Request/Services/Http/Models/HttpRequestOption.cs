namespace EssentialLayers.Request.Services.Http.Models
{
	public class HttpRequestOption
	{
		public string AppName { get; set; } = "AppName";

		public string AppVersion { get; set; } = "1.0";

		public string BearerToken { get; set; } = string.Empty;
	}
}