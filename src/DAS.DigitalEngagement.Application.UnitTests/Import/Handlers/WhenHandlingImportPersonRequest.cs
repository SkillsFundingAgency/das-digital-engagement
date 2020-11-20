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
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ImportStatus = DAS.DigitalEngagement.Models.BulkImport.ImportStatus;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Application.UnitTests.Import.Handlers
{
    public class WhenHandlingImportPersonRequest
    {
        private ImportPersonHandler _handler;
        private Mock<IChunkingService> _chunkingServiceMock;
        private Mock<ICsvService> _csvService;
        private Mock<IBulkImportService> _bulkImportService;
        private Mock<ILogger<ImportPersonHandler>> _logger;

        private IChunkingService _chunkingService;

        private string _testCsv = CsvTestHelper.GetValidCsv_SingleChunk();
        private List<dynamic> _testLeadList = GenerateNewLeads(10);

        [SetUp]
        public void Arrange()
        {
            _chunkingServiceMock = new Mock<IChunkingService>();
            _csvService = new Mock<ICsvService>();
            _bulkImportService = new Mock<IBulkImportService>();
            _logger = new Mock<ILogger<ImportPersonHandler>>();


            _csvService.Setup(s => s.ConvertToList(It.IsAny<StreamReader>())).ReturnsAsync(_testLeadList);
            _chunkingServiceMock.Setup(s => s.GetChunks(It.IsAny<int>(), _testLeadList))
                .Returns(new List<IList<dynamic>>());
            _bulkImportService.Setup(s => s.ImportPeople(It.IsAny<IList<dynamic>>())).ReturnsAsync(new BulkImportStatus());
            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>()))
                .ReturnsAsync(new FieldValidationResult());

            var marketoConfig = new Mock<IOptions<MarketoConfiguration>>();
            marketoConfig.Setup(x => x.Value.ChunkSizeKB).Returns(10000);    // 10MB chunk size
            _chunkingService = new ChunkingService(marketoConfig.Object);

            _handler = new ImportPersonHandler(_csvService.Object, _bulkImportService.Object, _logger.Object);
        }

        [Test]
        public async Task When_Fields_Valid_Then_The_Blob_Is_Converted_To_List()
        {
            //Arrange
            _csvService.Setup(s => s.HasData(It.IsAny<StreamReader>())).Returns(true);

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream);
            }

            //Assert
            _csvService.Verify(s => s.ConvertToList(It.IsAny<StreamReader>()), Times.Once);
        }


        [Test]
        public async Task When_Fields_Valid_Then_The_List_Is_Sent_To_Marketo()
        {
            var noOfLeads = 700000;
            var Leads = GenerateNewLeads(noOfLeads);
            
            _csvService.Setup(s => s.HasData(It.IsAny<StreamReader>())).Returns(true);
            _csvService.Setup(s => s.ConvertToList(It.IsAny<StreamReader>())).ReturnsAsync(Leads);

            _chunkingServiceMock.Setup(s => s.GetChunks(172, Leads))
                .Returns(_chunkingService.GetChunks(28000000, Leads));

            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                await _handler.Handle(test_Stream);
            }

            //Assert
            _bulkImportService.Verify(s => s.ImportPeople(It.IsAny<List<dynamic>>()), Times.Once);
        }

        [Test]
        public async Task When_Fields_Invalid_Then_The_Blob_Is_not_Converted_To_List()
        {
            //Arrange
            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>())).ReturnsAsync(new FieldValidationResult()
            {
                Errors = new List<string>() { "FirstName", "LastName" }
            });

            BulkImportStatus result;
            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                result = await _handler.Handle(test_Stream);
            }

            //Assert
            _csvService.Verify(s => s.ConvertToList(It.IsAny<StreamReader>()), Times.Never);
        }

        [Test]
        public async Task When_Fields_Invalid_Then_The_List_Is_Not_Sent_To_Marketo()
        {
            //Arrange
            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>())).ReturnsAsync(new FieldValidationResult()
            {
                Errors = new List<string>() { "FirstName", "LastName" }
            });

            BulkImportStatus result;
            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                result = await _handler.Handle(test_Stream);
            }

            //Assert
            _bulkImportService.Verify(s => s.ImportPeople(It.IsAny<List<dynamic>>()), Times.Never);
        }

        [Test]
        public async Task When_Fields_Invalid_Then_Response_Has_Invalid_status()
        {
            //Arrange
            _bulkImportService.Setup(s => s.ValidateFields(It.IsAny<IList<string>>())).ReturnsAsync(
                new FieldValidationResult()
                {
                    Errors = new List<string>() {"FirstName", "LastName"}
                });

            BulkImportStatus result;
            //Act
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(_testCsv)))
            {
                result = await _handler.Handle(test_Stream);
            }

            result.Should().NotBeNull();
            result.Status.Should().Be(ImportStatus.ValidationFailed);
            result.HeaderErrors.Should().BeEmpty();
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
