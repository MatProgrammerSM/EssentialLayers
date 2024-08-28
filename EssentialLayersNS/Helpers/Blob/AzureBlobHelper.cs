using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EssentialLayers.Helpers.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Response = Azure.Response;

namespace EssentialLayers.Helpers.Blob
{
	public sealed class AzureBlobHelper(string connectionString)
	{
		private readonly BlobServiceClient BlobServiceClient = new(connectionString);

		public async Task<ResultHelper<string>> UploadFileAsync(
			string fileName, string container, byte[] bytes
		)
		{
			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
				BinaryData binaryData = new(bytes);

				Response<BlobContentInfo> result = await blobClient.UploadAsync(binaryData, true);

				if (result.Value != null) return ResultHelper<string>.Success(
					$"{blobClient.Uri.AbsoluteUri}"
				);
				else return ResultHelper<string>.Fail(
					"File isn't uploaded successfully"
				);
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
				BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);
				BlobClient blobClient = blobContainerClient.GetBlobClient(filepath);

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
				BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);

				BlobClient blobClient = blobContainerClient.GetBlobClient(filepath);

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

		public async Task<ResultHelper<byte[]>> DowloadBytesAsync(
			string filepath, string container
		)
		{
			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);
				BlobClient blobClient = blobContainerClient.GetBlobClient(filepath);

				await using MemoryStream memoryStream = new();

				using Response result = await blobClient.DownloadToAsync(memoryStream);

				if (!result.IsError) return ResultHelper<byte[]>.Success(
					memoryStream.ToArray()
				);
				else return ResultHelper<byte[]>.Fail(
					"It wasn't possible dowloaded the file"
				);
			}
			catch (Exception e)
			{
				return ResultHelper<byte[]>.Fail(e);
			}
		}

		public async Task<ResultHelper<bool>> DeleteFileAsync(
			string filepath, string container
		)
		{
			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);
				Response<bool> result = await blobContainerClient.DeleteBlobIfExistsAsync(filepath);

				if (result) return ResultHelper<bool>.Success(true);
				else return ResultHelper<bool>.Fail(
					"It wasn't possible dowloaded the file"
				);
			}
			catch (Exception e)
			{
				return ResultHelper<bool>.Fail(e);
			}
		}

		public async Task<HashSet<BlobItem>> ListContainersAsync(
			string container, string prefix
		)
		{
			BlobContainerClient blobContainerClient = BlobServiceClient.GetBlobContainerClient(container);
			AsyncPageable<BlobItem> blobItems = blobContainerClient.GetBlobsAsync(prefix: prefix);

			HashSet<BlobItem> result = [];

			await foreach (BlobItem blobItem in blobItems)
			{
				result.Add(blobItem);
			}

			return result;
		}
	}
}