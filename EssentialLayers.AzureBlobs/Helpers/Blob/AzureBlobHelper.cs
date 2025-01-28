using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EssentialLayers.AzureBlobs.Helpers.Blob
{
	public class AzureBlobHelper(string connectionString)
	{
		private readonly BlobServiceClient? BlobServiceClient;

		/**/

		private readonly string? ConnectionString = connectionString;

		/**/

		public async Task<ResultHelper<string>> UploadFileAsync(
			string fileName, string container, byte[] bytes
		)
		{
			ResultHelper<string> result = GlobalFunctions.ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GlobalFunctions.GetBlobClient(BlobServiceClient, container, fileName);
				BinaryData binaryData = new(bytes);

				Response<BlobContentInfo> uplaod = await blobClient.UploadAsync(binaryData, true);

				GlobalFunctions.Info("ListContainersAsync", container, fileName);

				if (uplaod.Value != null) return ResultHelper<string>.Success(
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
			ResultHelper<string> result = GlobalFunctions.ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GlobalFunctions.GetBlobClient(BlobServiceClient, container, filepath);

				GlobalFunctions.Info("GetUrlFile", container, filepath);

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
			ResultHelper<string> result = GlobalFunctions.ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GlobalFunctions.GetBlobClient(BlobServiceClient, container, filepath);

				using Response download = await blobClient.DownloadToAsync(filepath);

				GlobalFunctions.Info("DowloadFileAsync", container, filepath);

				if (!download.IsError) return ResultHelper<string>.Success(
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
			ResultHelper<byte[]> result = GlobalFunctions.ValidateConnectionString<byte[]>(
				[], ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GlobalFunctions.GetBlobClient(BlobServiceClient, container, filepath);

				await using MemoryStream memoryStream = new();

				using Response download = await blobClient.DownloadToAsync(memoryStream);

				GlobalFunctions.Info("DowloadBytesAsync", container, filepath);

				if (!download.IsError) return ResultHelper<byte[]>.Success(
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
			ResultHelper<bool> result = GlobalFunctions.ValidateConnectionString(
			   true, ConnectionString!
		   );

			if (result.Ok.False()) return result;

			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient!.GetBlobContainerClient(container);
				Response<bool> deleted = await blobContainerClient.DeleteBlobIfExistsAsync(filepath);

				GlobalFunctions.Info("DeleteFileAsync", container, filepath);

				if (deleted) return ResultHelper<bool>.Success(true);
				else return ResultHelper<bool>.Fail(
					"It wasn't possible dowloaded the file"
				);
			}
			catch (Exception e)
			{
				return ResultHelper<bool>.Fail(e);
			}
		}

		public async Task<ResultHelper<HashSet<BlobItem>>> ListContainersAsync(
			string container, string prefix
		)
		{
			ResultHelper<HashSet<BlobItem>> result = GlobalFunctions.ValidateConnectionString<HashSet<BlobItem>>(
				[], ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient!.GetBlobContainerClient(container);
				AsyncPageable<BlobItem> blobItems = blobContainerClient.GetBlobsAsync(prefix: prefix);

				HashSet<BlobItem> hashSet = [];

				await foreach (BlobItem blobItem in blobItems)
				{
					hashSet.Add(blobItem);
				}

				string fileNames = hashSet.Select(x => x.Name).Join(", ");

				GlobalFunctions.Info("ListContainersAsync", container, fileNames);

				return ResultHelper<HashSet<BlobItem>>.Success(hashSet);
			}
			catch (Exception e)
			{
				return ResultHelper<HashSet<BlobItem>>.Fail(e);
			}
		}
	}
}