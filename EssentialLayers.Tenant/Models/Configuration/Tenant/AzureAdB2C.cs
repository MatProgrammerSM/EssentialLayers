namespace EssentialLayers.Tenant.Models.Configuration.Tenant
{
	public class AzureAdB2C
	{
		public string Instance { get; set; } = string.Empty;

		public string ClientId { get; set; } = string.Empty;

		public string Domain { get; set; } = string.Empty;

		public string SignedOutCallbackPath { get; set; } = string.Empty;

		public string SignUpSignInPolicyId { get; set; } = string.Empty;

		public string ResetPasswordPolicyId { get; set; } = string.Empty;

		public string ClientSecret { get; set; } = string.Empty;
	}
}