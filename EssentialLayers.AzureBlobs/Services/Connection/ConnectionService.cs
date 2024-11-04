namespace EssentialLayers.AzureBlobs.Services.Connection
{
	internal class ConnectionService : IConnectionService
	{
		private string ConnectionString = string.Empty;

		public string Get() => ConnectionString;

		public void Set(string connectionString)
		{
			ConnectionString = connectionString;
		}
	}
}