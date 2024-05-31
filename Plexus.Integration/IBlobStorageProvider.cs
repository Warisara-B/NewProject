
namespace Plexus.Integration
{
    public interface IBlobStorageProvider
    {
        /// <summary>
        /// Get public url generate from given {filename} along with accessKey which expired in {expiryDay} day
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="expiryDay"></param>
        /// <returns></returns>
        string? GetBlobPublicUrl(string? filename, int expiryDay = 1);

        /// <summary>
        /// Upload file to storage blob with specific path from {pathAndFilename}
        /// </summary>
        /// <param name="pathAndFilename"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        Task UploadFileAsync(string pathAndFilename, Stream fileStream);
    }
}