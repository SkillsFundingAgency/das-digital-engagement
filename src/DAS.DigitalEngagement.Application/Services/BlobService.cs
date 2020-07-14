using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Services
{
    public interface IBlobService
    {
        Task DeleteFile(string uniqueFileIdentifier, string containerName);
    }

    public class BlobService : IBlobService
    {
        private readonly ILogger<IBlobService> _logger;
        private IBlobContainerClientWrapper _blobContainerClientWrapper;

        public BlobService(ILogger<IBlobService> logger, IBlobContainerClientWrapper blobContainerClientWrapper)
        {
            _logger = logger;
            _blobContainerClientWrapper = blobContainerClientWrapper;
        }

        public async Task DeleteFile(string uniqueFileIdentifier, string containerName)
        {
           await _blobContainerClientWrapper.DeleteBlobAsync(uniqueFileIdentifier, containerName);
        }
    }

    public interface IBlobClientWrapper
    {
        IBlobContainerClientWrapper GetBlobContainerClient(string blobContainerName);
    }


    public interface IBlobContainerClientWrapper
    {
        Task<Response> DeleteBlobAsync(string blobName, string containerName);
    }
}