using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Sas;
using ServicePlatform.Interface;
using Microsoft.Extensions.Options;
using Azure.Storage;

namespace ServicePlatform.Services
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
	public class BlobService
	{
		private readonly AzureStorageConfig _azureStorageConfig;
        private readonly BlobServiceClient _blobServiceClient;

		public BlobService(IOptions<AzureStorageConfig> azureStorageConfig)
		{
			_azureStorageConfig = azureStorageConfig.Value;
			_blobServiceClient = new BlobServiceClient(azureStorageConfig.Value.AzureConnectionString);
		}

		//[HttpPost]
		//public async Task UploadAsync(IFormFile file)
		//{
		//    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		//    await containerClient.CreateIfNotExistsAsync();

		//    var blobClient = containerClient.GetBlobClient(file.FileName);
		//    using (var stream = file.OpenReadStream())
		//    {
		//        await blobClient.UploadAsync(stream, true);
		//    }
		//}

		//[HttpGet]
		//public async Task<Stream> DownloadAsync(string fileName)
		//{
		//    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		//    var blobClient = containerClient.GetBlobClient(fileName);

		//    var response = await blobClient.OpenReadAsync();
		//    return response;
		//}


		[HttpPost]
		public async Task<string> UploadAsync(IFormFile file, string azureBlobContainer)
		{
			// Generate a new GUID to use as the filename
			string newFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

			var containerClient = _blobServiceClient.GetBlobContainerClient(azureBlobContainer);
			await containerClient.CreateIfNotExistsAsync();

			//var blobClient = containerClient.GetBlobClient(file.FileName);
			var blobClient = containerClient.GetBlobClient(newFileName);

            // Determine the content type of the uploaded file
            string contentType = file.ContentType;


            using (var stream = file.OpenReadStream())
            {
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType // Set the content type
                };

                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }
            return newFileName;
		}

		[HttpGet]
		public async Task<string> GetSasUriAsync(string fileName, string azureBlobContainer)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(azureBlobContainer);
			var blobClient = containerClient.GetBlobClient(fileName);

			var sasBuilder = new BlobSasBuilder()
			{
				BlobContainerName = azureBlobContainer,
				BlobName = fileName,
				ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(20), // Set the expiry time as needed
				StartsOn = DateTimeOffset.UtcNow.AddMinutes(-15), // Set the start time as needed
				Protocol = SasProtocol.Https                      // Protocal Settings                            
																  // Set the required permissions using the SetPermissions method
																  // Permissions = BlobSasPermissions.Read
			};
			sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);

			var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

			// Return the complete SAS URI
			return blobClient.Uri + sasToken;
		}


		[HttpGet]
		public async Task<string> GetSasUriAsync_ImageView(string fileName, string azureBlobContainer, int minutes = 15)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(azureBlobContainer);
			var blobClient = containerClient.GetBlobClient(fileName);

			var sasBuilder = new BlobSasBuilder()
			{
				BlobContainerName = azureBlobContainer,
				BlobName = fileName,
				StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5), // Set the start time as needed
				ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(minutes), // Set the expiry time as needed
				Protocol = SasProtocol.Https,					  // Protocal Settings
																  // Set the required permissions using the SetPermissions method
																  // Permissions = BlobSasPermissions.Read
			};
			sasBuilder.SetPermissions(BlobSasPermissions.Read);

            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(
                    _azureStorageConfig.AccountName,
                    _azureStorageConfig.AccountKey);

            var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

			// Return the complete SAS URI
			return blobClient.Uri + sasToken;
		}



        [HttpGet]
		public async Task<Stream> DownloadAsync(string fileName, string azureBlobContainer)
		{
			var sasUri = await GetSasUriAsync(fileName, azureBlobContainer);

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetStreamAsync(sasUri);
				return response;
			}
		}


		public async Task DeleteAsync(string fileName, string azureBlobContainer)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(azureBlobContainer);
			var blobClient = containerClient.GetBlobClient(fileName);

			// Delete the blob if it exists
			await blobClient.DeleteIfExistsAsync();
		}





	}
}
