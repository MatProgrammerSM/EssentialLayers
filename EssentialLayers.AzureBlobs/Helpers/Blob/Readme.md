# Essential Layers
#### [EssentialLayers.AzureBlobs](/EssentialLayers.AzureBlobs/Readme.md)
It is a helper that performs the management of the blob service of azure, offers a number of methods to interact with containers where the blobs are stored.

| Method  | Definition |
| :--------- | :------------ |
| UploadFileAsync | `Task<ResultHelper<string>> UploadFileAsync(string fileName, string container, byte[] bytes);` |
| GetUrlFile | `ResultHelper<string> GetUrlFile(string filepath, string container);` |
| DowloadFileAsync | `Task<ResultHelper<string>> DowloadFileAsync(string filepath, string container);` |
| DowloadBytesAsync | `Task<ResultHelper<byte[]>> DowloadBytesAsync(string filepath, string container);` |
| DeleteFileAsync | `Task<ResultHelper<bool>> DeleteFileAsync(string filepath, string container);` |
| UploadFileAsync | `Task<ResultHelper<HashSet<BlobItem>>> ListContainersAsync(string container, string prefix);` |
```
Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)