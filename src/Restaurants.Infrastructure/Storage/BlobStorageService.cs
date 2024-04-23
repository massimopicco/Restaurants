using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
{


    private readonly BlobStorageSettings _blobSotrageSettings = blobStorageSettingsOptions.Value;


    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_blobSotrageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobSotrageSettings.LogosContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);

        var url = blobClient.Uri.ToString();
        return url;
    }


    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl is null) return null;

        var blobSasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobSotrageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };

        blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_blobSotrageSettings.ConnectionString);

        var sasToken = blobSasBuilder
            .ToSasQueryParameters(new StorageSharedKeyCredential(blobServiceClient.AccountName, _blobSotrageSettings.AccountKey))
            .ToString();

        return $"{blobUrl}?{sasToken}";
    }


    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }


}
