using Azure.Storage.Blobs.Models;
using EssentialLayers.AzureBlobs.Helpers;
using EssentialLayers.AzureBlobs.Helpers.Blob;
using EssentialLayers.Helpers.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayers.AzureBlobs.Services.Blob
{
	internal class AzureBlobService : IAzureBlobService
	{
		private readonly AzureBlobHelper azureBlobHelper;

		public AzureBlobService()
		{
			azureBlobHelper = new(Tools.Get.Connection!.Get());
		}

		public async Task<ResultHelper<string>> UploadFileAsync(
			string fileName, string container, byte[] bytes
		)
		{
			return await azureBlobHelper.UploadFileAsync(fileName, container, bytes);
		}

		public ResultHelper<string> GetUrlFile(
			string filepath, string container
		)
		{
			return azureBlobHelper.GetUrlFile(filepath, container);
		}

		public async Task<ResultHelper<string>> DowloadFileAsync(
			string filepath, string container
		)
		{
			return await azureBlobHelper.DowloadFileAsync(filepath, container);
		}

		public async Task<ResultHelper<byte[]>> DowloadBytesAsync(
			string filepath, string container
		)
		{
			return await azureBlobHelper.DowloadBytesAsync(filepath, container);
		}

		public async Task<ResultHelper<bool>> DeleteFileAsync(
			string filepath, string container
		)
		{
			return await azureBlobHelper.DeleteFileAsync(filepath, container);
		}

		public async Task<ResultHelper<HashSet<BlobItem>>> ListContainersAsync(
			string container, string prefix
		)
		{
			return await azureBlobHelper.ListContainersAsync(container, prefix);
		}
	}
}