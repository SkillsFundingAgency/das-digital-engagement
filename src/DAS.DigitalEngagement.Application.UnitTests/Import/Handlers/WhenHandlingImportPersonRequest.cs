using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Application.UnitTests.Helpers;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Services;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DAS.DigitalEngagement.Application.UnitTests.Import.Handlers
{
    public class WhenHandlingImportPersonRequest
    {
        private ImportPersonHandler _handler;
        private Mock<IChunkingService> _chunkingServiceMock;
        private Mock<ICsvService> _csvService;
        private Mock<IBulkImportService> _bulkImportService;
        private Mock<ILogger<ImportPersonHandler>> _logger;
        private Mock<IReportService> _reportService;

        private IChunkingService _chunkingService = new ChunkingService();

        private string _testCsv = CsvTestHelper.GetValidCsv_SingleChunk();
        private List<dynamic> _testLeadList = GenerateNewLeads(10);

        [SetUp]
        public void Arrange()
        {
            _chunkingServiceMock = new Mock<IChunkingService>();
            _csvService = new Mock<ICsvService>();
            _bulkImportService = new Mock<IBulkImportService>();
            _logger = new Mock<ILogger<ImportPersonHandler>>();
            _reportService = new Mock<IReportService>();


            _csvService.Setup(s => s.ConvertToList(It.IsAny<Stream>())).ReturnsAsync(_testLeadList);
            _chunkingServiceMock.Setup(s => s.GetChunks(It.IsAny<int>(), _testLeadList))
                .Returns(new List<IList<dynamic>>());
            _bulkImportService.Setup(s => s.ImportPeople(It.IsAny<IList<dynamic>>())).ReturnsAsync(new BulkImportJob()
            { batchId = 1, ImportId = "Imported", Status = "Queued" });


            _handler = new ImportPersonHandler(_chunkingServiceMock.Object, _csvService.Object, _bulkImportService.Object, _logger.Object, _reportService.Object);
        }

        [Test]
        public async Task Then_The_Blob_Is_Converted_To_List()
        {
            //Arrange

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream);
            }

            //Assert
            _csvService.Verify(s => s.ConvertToList(It.IsAny<Stream>()), Times.Once);
        }

        [Test]
        public async Task Then_The_List_is_Chunked_Into_Valid_Sizes()
        {
            //Arrange

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream);
            }

            //Assert
            _chunkingServiceMock.Verify(s => s.GetChunks(172, _testLeadList), Times.Once);
        }

        [Test]
        public async Task Then_The_Each_Chunk_Is_Sent_To_Marketo()
        {
            var noOfLeads = 700000;
            var Leads = GenerateNewLeads(noOfLeads);

            _csvService.Setup(s => s.ConvertToList(It.IsAny<Stream>())).ReturnsAsync(Leads);

            _chunkingServiceMock.Setup(s => s.GetChunks(172, Leads))
                .Returns(_chunkingService.GetChunks(28000000, Leads));

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream);
            }

            //Assert
            _bulkImportService.Verify(s => s.ImportPeople(It.IsAny<List<dynamic>>()), Times.AtLeast(2));
        }

        private static List<dynamic> GenerateNewLeads(int leadCount)
        {
            var Leads = Enumerable
                .Range(0, leadCount)
                .Select(i =>
                {
                    dynamic expando = new ExpandoObject();
                    expando.FirstName = $"Firstname{i}";
                    expando.LastName = "Surname ";
                    expando.Email = $"Firstname{i}.lastname@Email.com";
                    expando.Company = $"MyNewCompany{i}";
                    return (dynamic)expando;
                })
                .ToList();

            return Leads;
        }
    }
}
