using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DAS.DigitalEngagement.Application.UnitTests.Services
{
    public class BlobServiceTests
    {
        private BlobService _service;

        private Mock<IBlobContainerClientWrapper>  _blobContainerClientWraper;
        private Mock<ILogger<IBlobService>> _blobServiceLogger;

        [SetUp]
        public void Arrange()
        {
            _blobContainerClientWraper = new Mock<IBlobContainerClientWrapper>();
            _blobServiceLogger = new Mock<ILogger<IBlobService>>();
            _service = new BlobService(_blobServiceLogger.Object,_blobContainerClientWraper.Object);
        }

        [Test]
        public async Task When_Deleting_A_File_Then_Delete_On_Client_Is_Called()
        {
            var blobName = "FileNAme";
            var containerName = "Container";


            await _service.DeleteFile(blobName, containerName);


            _blobContainerClientWraper.Verify(v => v.DeleteBlobAsync(blobName,containerName),Times.Once);

        }

      
    }
}
