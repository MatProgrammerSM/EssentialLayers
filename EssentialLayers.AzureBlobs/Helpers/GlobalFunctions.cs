using Azure.Storage.Blobs;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using System.Diagnostics;

namespace EssentialLayers.AzureBlobs.Helpers
{
	internal class GlobalFunctions
	{
		public static ResultHelper<TResult> ValidateConnectionString<TResult>(
			TResult result, string connectionString
		)
		{
			bool isEmpty = connectionString.IsEmpty();

			if (isEmpty) return ResultHelper<TResult>.Fail(
				"The connection string wasn't initilized yet"
			);

			return ResultHelper<TResult>.Success(result);
		}

		public static BlobClient GetBlobClient(BlobServiceClient? blobServiceClient, string container, string filepath)
		{
			BlobContainerClient blobContainerClient = blobServiceClient!.GetBlobContainerClient(container);
			BlobClient blobClient = blobContainerClient.GetBlobClient(filepath);

			return blobClient;
		}

		public static BlobServiceClient GetBlobServiceClient(string connectionString)
		{
			return new BlobServiceClient(connectionString);
		}

		public static void Info(string method, string container, string filename)
		{
			Trace.WriteLine($"[EssentialLayers.AzureBlobs] - Method: {method}, Container: {container}, Filename: {filename}");
		}
	}
}