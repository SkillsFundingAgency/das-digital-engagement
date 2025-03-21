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
using DAS.DigitalEngagement.Models.BulkImport;
using DAS.DigitalEngagement.Models.Validation;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Application.UnitTests.Import.Handlers
{
    public class WhenHandlingImportCampaignMemberRequest
    {
        private ImportCampaignMembersHandler _handler;
        private Mock<IChunkingService> _chunkingServiceMock;
        private Mock<ICsvService> _csvService;
        private Mock<IBulkImportService> _bulkImportService;
        private Mock<ILogger<ImportCampaignMembersHandler>> _logger;
        private Mock<IReportService> _reportService;

        private IChunkingService _chunkingService;

        private string _testCsv = CsvTestHelper.GetValidCsv_SingleChunk();
        private IList<dynamic> _testLeadList = GenerateNewLeads(10);

        [SetUp]
        public void Arrange()
        {
            _chunkingServiceMock = new Mock<IChunkingService>();
            _csvService = new Mock<ICsvService>();
            _bulkImportService = new Mock<IBulkImportService>();
            _logger = new Mock<ILogger<ImportCampaignMembersHandler>>();
            _reportService = new Mock<IReportService>();
            var marketoConfig = new Mock<IOptions<MarketoConfiguration>>();
            marketoConfig.Setup(x => x.Value.ChunkSizeKB).Returns(10000);    // 10MB chunk size

            _chunkingService = new ChunkingService(marketoConfig.Object);
            _csvService.Setup(s => s.ConvertToList(It.IsAny<StreamReader>())).ReturnsAsync(_testLeadList);
            _chunkingServiceMock.Setup(s => s.GetChunks(It.IsAny<int>(),_testLeadList))
                .Returns(new List<IList<dynamic>>());
            _bulkImportService.Setup(s => s.ImportPeople(It.IsAny<IList<dynamic>>())).ReturnsAsync(new BulkImportStatus());
            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>()))
                .ReturnsAsync(new FieldValidationResult());

            _handler = new ImportCampaignMembersHandler(_chunkingServiceMock.Object,_csvService.Object,_bulkImportService.Object,_logger.Object);
        }

        [Test]
        public async Task Then_The_Blob_Is_Converted_To_List()
        {
            //Arrange
            var campaignId = "campaignId";
            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream, campaignId);
            }
           
            //Assert
            _csvService.Verify(s => s.ConvertToList(It.IsAny<StreamReader>()),Times.Once);
        }

        [Test]
        public async Task Then_The_List_is_Chunked_Into_Valid_Sizes()
        {
            //Arrange
            var campaignId = "campaignId";

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream, campaignId);
            }

            //Assert
            _chunkingServiceMock.Verify(s => s.GetChunks(It.IsAny<long>(), _testLeadList), Times.Once);
        }

        [Test]
        public async Task Then_The_Each_Chunk_Is_Sent_To_Marketo()
        {
            var noOfLeads = 700000;
            var Leads = GenerateNewLeads(noOfLeads);
            var campaignId = "campaignId";

            _csvService.Setup(s => s.ConvertToList(It.IsAny<StreamReader>())).ReturnsAsync(Leads);

            _csvService.Setup(s => s.IsEmpty(It.IsAny<StreamReader>())).Returns(false);
            _csvService.Setup(s => s.HasData(It.IsAny<StreamReader>())).Returns(false);

            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>()))
                .ReturnsAsync(new FieldValidationResult
                {
                    Errors = new List<string>() 
                });
            
            _chunkingServiceMock.Setup(s => s.GetChunks(It.IsAny<long>(), Leads))
                .Returns(_chunkingService.GetChunks(28000000, Leads));

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream, campaignId);
            }

            //Assert
            _bulkImportService.Verify(s => s.ImportToCampaign(It.IsAny<IList<dynamic>>(), "campaignId"), Times.AtLeast(2));
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
