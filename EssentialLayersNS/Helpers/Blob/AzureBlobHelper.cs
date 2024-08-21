using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EssentialLayersNS.Helpers.Result;
using System;
using System.Threading.Tasks;
using Response = Azure.Response;

namespace EssentialLayersNS.Helpers.Blob
{
	public sealed class AzureBlobHelper(string connectionString)
	{
		private readonly BlobServiceClient BlobServiceClient = new(connectionString);

		private BlobContainerClient BlobContainerClient;

		public async Task<ResultHelper<string>> UploadFileAsync(
			string fileName, string container, byte[] bytes
		)
		{
			try
			{
				BlobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				BlobClient blobClient = BlobContainerClient.GetBlobClient(fileName);
				BinaryData binaryData = new(bytes);

				Response<BlobContentInfo> result = await blobClient.UploadAsync(binaryData, true);

				if (result.Value != null) return ResultHelper<string>.Success(
					$"{blobClient.Uri.AbsoluteUri}"
				);
				else return ResultHelper<string>.Fail("File isn't uploaded successfully");
			}
			catch (Exception e)
			{
				return ResultHelper<string>.Fail(e);
			}
		}

		public ResultHelper<string> GetUrlFile(
			string filepath, string container
		)
		{
			try
			{
				BlobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				BlobClient blobClient = BlobContainerClient.GetBlobClient(filepath);

				return ResultHelper<string>.Success(
					$"{blobClient.Uri.AbsoluteUri}"
				);
			}
			catch (Exception e)
			{
				return ResultHelper<string>.Fail(e);
			}
		}

		public async Task<ResultHelper<string>> DowloadFileAsync(
			string filepath, string container
		)
		{
			try
			{
				BlobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				BlobClient blobClient = BlobContainerClient.GetBlobClient(filepath);

				using Response result = await blobClient.DownloadToAsync(filepath);

				if (!result.IsError) return ResultHelper<string>.Success(
					$"{blobClient.Uri.AbsoluteUri}"
				);
				else return ResultHelper<string>.Fail("It wasn't possible dowloaded the file");
			}
			catch (Exception e)
			{
				return ResultHelper<string>.Fail(e);
			}
		}

		public async Task<ResultHelper<bool>> DeleteFileAsync(
			string filepath, string container
		)
		{
			try
			{
				BlobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				Response<bool> result = await BlobContainerClient.DeleteBlobIfExistsAsync(filepath);

				if (result) return ResultHelper<bool>.Success(true);
				else return ResultHelper<bool>.Fail("It wasn't possible dowloaded the file");
			}
			catch (Exception e)
			{
				return ResultHelper<bool>.Fail(e);
			}
		}
	}
}