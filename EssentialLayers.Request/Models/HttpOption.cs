namespace EssentialLayers.Request.Models
{
	public class HttpOption
	{
		public string AppName { get; set; } = "AppName";

		public string AppVersion { get; set; } = "1.0";

		public string BaseUri { get; set; } = string.Empty;

		public bool CastResultAsResultHelper { get; set; } = false;

		public bool InsensitiveMapping { get; set; } = true;
    }
}