using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;

namespace DAS.DigitalEngagement.Application.Services
{
    public class BlobContainerClientWrapper : IBlobContainerClientWrapper
    {
        private BlobContainerClient _blobContainerClient;
        private string _connectionString;

        public BlobContainerClientWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<Response> DeleteBlobAsync(string blobName, string containerName)
        {
            _blobContainerClient = new BlobContainerClient(_connectionString, containerName);

            return _blobContainerClient.DeleteBlobAsync(blobName);
        }
    }
}