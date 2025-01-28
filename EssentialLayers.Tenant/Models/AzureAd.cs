namespace EssentialLayers.Tenant.Models
{
	public class AzureAd
	{
		public string TenantId { get; set; } = string.Empty;

		public string ClientId { get; set; } = string.Empty;

		public string ClientSecret { get; set; } = string.Empty;
	}
}