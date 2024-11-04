using Azure.Storage.Blobs.Models;
using EssentialLayers.Helpers.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayers.AzureBlobs.Services.Blob;

public interface IAzureBlobService
{
	Task<ResultHelper<string>> UploadFileAsync(
		string fileName, string container, byte[] bytes
	);

	ResultHelper<string> GetUrlFile(
		string filepath, string container
	);

	Task<ResultHelper<string>> DowloadFileAsync(
		string filepath, string container
	);

	Task<ResultHelper<byte[]>> DowloadBytesAsync(
		string filepath, string container
	);

	Task<ResultHelper<bool>> DeleteFileAsync(
		string filepath, string container
	);

	Task<ResultHelper<HashSet<BlobItem>>> ListContainersAsync(
		string container, string prefix
	);
}