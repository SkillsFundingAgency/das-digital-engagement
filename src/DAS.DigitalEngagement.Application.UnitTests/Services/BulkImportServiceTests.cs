using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Application.Services.Marketo;
using DAS.DigitalEngagement.Application.UnitTests.Helpers;
using DAS.DigitalEngagement.Domain.Mapping.BulkImport;
using DAS.DigitalEngagement.Domain.Services;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;
using FluentAssertions;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Refit;

namespace DAS.DigitalEngagement.Application.UnitTests.Services
{
    public class BulkImportServiceTests
    {
        private MarketoBulkImportService _service;
        private Mock<IMarketoLeadClient> _marketoLeadClientMock;
        private Mock<IMarketoBulkImportClient> _marketoBulkImportClientMock;
        private Mock<ICsvService> _csvServiceMock;
        private Mock<IBulkImportJobMapper> _bulkImportJobMapperMock;
        private Mock<IBulkImportStatusMapper> _bulkImportStatusMapperMock;
        private Mock<IChunkingService> _chunkingServiceMock;

        private Mock<ILogger<MarketoBulkImportService>> _logger;

        private IChunkingService _chunkingService = new ChunkingService();

        private string _testCsv = CsvTestHelper.GetValidCsv_SingleChunk();
        private List<dynamic> _testLeadList = GenerateNewLeads(10);

        [SetUp]
        public void Arrange()
        {
            _marketoLeadClientMock = new Mock<IMarketoLeadClient>();
            _marketoBulkImportClientMock = new Mock<IMarketoBulkImportClient>();
            _bulkImportJobMapperMock = new Mock<IBulkImportJobMapper>();
            _bulkImportStatusMapperMock = new Mock<IBulkImportStatusMapper>();
            _chunkingServiceMock = new Mock<IChunkingService>();
            _csvServiceMock = new Mock<ICsvService>();
            _logger = new Mock<ILogger<MarketoBulkImportService>>();


            var jobResponse = new Response<BatchJob>()
            {
                RequestId = "RequestId",
                Success = true,
                Result = new List<BatchJob>()
            };

            _chunkingServiceMock.Setup(s => s.GetChunks(172, _testLeadList))
                .Returns(_chunkingService.GetChunks(172, _testLeadList));
            _csvServiceMock.Setup(s => s.GetByteCount(_testLeadList)).Returns(172);
            _marketoBulkImportClientMock.Setup(s => s.PushLeads(It.IsAny<StreamPart>())).ReturnsAsync(jobResponse);

            _marketoLeadClientMock.Setup(s => s.Describe()).ReturnsAsync(GenerateDescribeReturn());


            _service = new MarketoBulkImportService(_marketoLeadClientMock.Object,
                                            _marketoBulkImportClientMock.Object,
                                            _csvServiceMock.Object,
                                            _logger.Object,
                                            _bulkImportStatusMapperMock.Object,
                                            _bulkImportJobMapperMock.Object, _chunkingServiceMock.Object);
        }


        [Test]
        public async Task When_Valid_Headers_Validated_Then_Return_Validtion_Passed()
        {
            var fields = new List<string>()
            {
                "attribute-1",
                "Attribute-2",
                "Attribute-3"
            };

            var validationResult = await _service.ValidateFields(fields);

            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task When_Invalid_Headers_Validated_Then_Return_Validtion_failed()
        {
            var fields = new List<string>()
            {
                "attribute-1",
                "Attribute-2",
                "Attribute-failed"
            };

            var validationResult = await _service.ValidateFields(fields);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task Then_The_List_is_Chunked_Into_Valid_Sizes()
        {
            //Arrange

            //Act
            var status = await _service.ImportPeople(_testLeadList);
            //Assert
            _chunkingServiceMock.Verify(s => s.GetChunks(172, _testLeadList), Times.Once);
        }

        [Test]
        public async Task Then_The_Each_Chunk_Is_Sent_To_Marketo()
        {
            //Act
            var status = await _service.ImportPeople(_testLeadList);


            //Assert
            status.Should().NotBeNull();
            status.BulkImportJobs.Should().HaveCount(1);

            _marketoBulkImportClientMock.Verify(v => v.PushLeads(It.IsAny<StreamPart>()), Times.Once());
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

        private static Response<LeadAttribute> GenerateDescribeReturn()
        {
            var result = new Response<LeadAttribute>();

            result.Success = true;
            result.Result = new List<LeadAttribute>();

            var attributes = Enumerable.Range(0, 20)
                .Select(i =>
                {
                    var attribute = new LeadAttribute();
                    attribute.DisplayName = "Attribute";
                    attribute.Soap = new LeadMapAttribute()
                    {
                        Name = $"attribute-soap-{i}"
                    };

                    attribute.Rest = new LeadMapAttribute()
                    {
                        Name = $"Attribute-{i}"
                    };
                    return attribute;
                })
                .ToList();

            result.Result = attributes;

            return result;
        }
    }
}
