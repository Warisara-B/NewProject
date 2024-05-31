using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Plexus.Integration.config;

namespace Plexus.Integration.src
{
    public class BlobStorageProvider : IBlobStorageProvider
    {
        private readonly BlobStorageConfiguration _configuration;

        private BlobContainerClient _containerClient;

        public BlobStorageProvider(IOptions<BlobStorageConfiguration> configurationOptions)
        {
            _configuration = configurationOptions.Value;

            _containerClient = new BlobContainerClient(_configuration.ConnectionString,
                                                       _configuration.ContainerName);
        }

        public async Task UploadFileAsync(string filename, Stream fileStream)
        {
            var blobClient = _containerClient.GetBlobClient(filename);

            await blobClient.UploadAsync(fileStream, true);
        }

        public string? GetBlobPublicUrl(string? filename, int expiryDay)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            var blockClient = _containerClient.GetBlobClient(filename);

            if (!blockClient.CanGenerateSasUri)
            {
                return null;
            }

            var uri = blockClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTime.UtcNow.AddDays(expiryDay));

            return uri.AbsoluteUri.Replace(uri.Query, "");
        }
    }
}

