﻿namespace EssentialLayers.AzureBlobs.Services.Connection
{
	public interface IConnectionService
	{
		void Set(string connectionString);

		string Get();
	}
}