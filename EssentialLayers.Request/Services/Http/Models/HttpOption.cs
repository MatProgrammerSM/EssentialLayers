namespace EssentialLayers.Request.Services.Http.Models
{
	public class HttpOption
	{
		public string AppName { get; set; } = "AppName";

		public string AppVersion { get; set; } = "1.0";

		public bool CastResultAsResultHelper { get; set; } = false;
	}
}