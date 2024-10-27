using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Response = Azure.Response;

namespace EssentialLayers.AzureBlobs.Services.Blob
{
	internal class AzureBlobService : IAzureBlobService
	{
		private BlobServiceClient? BlobServiceClient;

		private string? ConnectionString;

		public void SetConnectionString(string connectionString)
		{
			ConnectionString = connectionString;
			BlobServiceClient = new(connectionString);
		}

		public async Task<ResultHelper<string>> UploadFileAsync(
			string fileName, string container, byte[] bytes
		)
		{
			ResultHelper<string> result = ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GetBlobClient(container, fileName);
				BinaryData binaryData = new(bytes);

				Response<BlobContentInfo> uplaod = await blobClient.UploadAsync(binaryData, true);

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
			ResultHelper<string> result = ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GetBlobClient(container, filepath);

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
			ResultHelper<string> result = ValidateConnectionString(
				string.Empty, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GetBlobClient(container, filepath);

				using Response download = await blobClient.DownloadToAsync(filepath);

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
			ResultHelper<byte[]> result = ValidateConnectionString<byte[]>(
				[], ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobClient blobClient = GetBlobClient(container, filepath);

				await using MemoryStream memoryStream = new();

				using Response download = await blobClient.DownloadToAsync(memoryStream);

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
			ResultHelper<bool> result = ValidateConnectionString(
				true, ConnectionString!
			);

			if (result.Ok.False()) return result;

			try
			{
				BlobContainerClient blobContainerClient = BlobServiceClient!.GetBlobContainerClient(container);
				Response<bool> deleted = await blobContainerClient.DeleteBlobIfExistsAsync(filepath);

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
			ResultHelper<HashSet<BlobItem>> result = ValidateConnectionString<HashSet<BlobItem>>(
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

				return ResultHelper<HashSet<BlobItem>>.Success(hashSet);
			}
			catch (Exception e)
			{
				return ResultHelper<HashSet<BlobItem>>.Fail(e);
			}
		}

		private ResultHelper<TResult> ValidateConnectionString<TResult>(
			TResult result, string connectionString
		)
		{
			bool isEmpty = connectionString.IsEmpty();

			if (isEmpty) return ResultHelper<TResult>.Fail(
				"The connection string wasn't initilized yet"
			);

			return ResultHelper<TResult>.Success(result);
		}

		private BlobClient GetBlobClient(string container, string filepath)
		{
			BlobContainerClient blobContainerClient = BlobServiceClient!.GetBlobContainerClient(container);
			BlobClient blobClient = blobContainerClient.GetBlobClient(filepath);

			return blobClient;
		}
	}
}