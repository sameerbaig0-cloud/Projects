using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ServicePlatform.Interface;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

public class ArticlePictureService : IArticlePictureService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _accountName;
    private readonly string _accountKey;
    private readonly string _containerName;

    public ArticlePictureService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:AzureConnectionString"];
        _blobServiceClient = new BlobServiceClient(connectionString);
        _accountName = configuration["AzureStorage:AccountName"];
        _accountKey = configuration["AzureStorage:AccountKey"];
        _containerName = configuration.GetValue<string>("ProductionStylesBlobStorageContainer");
    }


    public async Task<List<string>> GetArticlePictureUrlsAsync(List<string> blobFileNames)
    {
        var urls = new List<string>();

        foreach (var blobFileName in blobFileNames)
        {
            try
            {
                var sasUri = await GetSasUriAsync(blobFileName, _containerName);
                urls.Add(sasUri);
            }
            catch (Exception ex)
            {
                // Log the detailed error message
                Console.WriteLine($"Error retrieving blob '{blobFileName}': {ex.Message}");
            }
        }

        return urls;
    }


    private async Task<string> GetSasUriAsync(string fileName, string azureBlobContainer, int minutes = 15)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(azureBlobContainer);
        var blobClient = containerClient.GetBlobClient(fileName);

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = azureBlobContainer,
            BlobName = fileName,
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5), // Set the start time as needed
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(minutes), // Set the expiry time as needed
            Protocol = SasProtocol.Https, // Protocal Settings
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var storageSharedKeyCredential = new StorageSharedKeyCredential(_accountName, _accountKey);
        var sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

        // Return the complete SAS URI
        return blobClient.Uri + "?" + sasToken;
    }
}
