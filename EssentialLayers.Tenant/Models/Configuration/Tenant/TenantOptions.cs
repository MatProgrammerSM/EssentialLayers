namespace EssentialLayers.Tenant.Models.Configuration.Tenant
{
	public class TenantOptions
	{
		public AzureAd? AzureAd { get; set; }

		public AzureAdB2C? AzureAdB2C { get; set; }
	}
}