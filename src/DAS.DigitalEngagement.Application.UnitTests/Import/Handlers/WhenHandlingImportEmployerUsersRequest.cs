using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Application.UnitTests.Helpers;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Infrastructure.Models;
using DAS.DigitalEngagement.Models;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Application.UnitTests.Import.Handlers
{
    public class WhenHandlingImportEmployerUsersRequest
    {
        private ImportEmployerUsersHandler _handler;
        private Mock<IBulkImportService> _bulkImportServiceMock;
        private Mock<ILogger<ImportEmployerUsersHandler>> _logger;
        private Mock<IEmployerUsersRepository> _employerUsersRepositoryMock;
        private Mock<IPersonMapper> _PersonMapperMock;


        private IChunkingService _chunkingService;

        private string _testCsv = CsvTestHelper.GetValidCsv_SingleChunk();
        private IList<EmployerUser> _testUserList = GenerateNewUsers(10);

        [SetUp]
        public void Arrange()
        {
            _bulkImportServiceMock = new Mock<IBulkImportService>();
            _logger = new Mock<ILogger<ImportEmployerUsersHandler>>();
            _employerUsersRepositoryMock = new Mock<IEmployerUsersRepository>();
            _PersonMapperMock = new Mock<IPersonMapper>();

            _bulkImportServiceMock.Setup(s => s.ImportPeople(It.IsAny<IList<dynamic>>())).ReturnsAsync(new BulkImportStatus());
            _employerUsersRepositoryMock.Setup(s => s.GetAllUsers()).ReturnsAsync(_testUserList);
            _PersonMapperMock.Setup(s => s.Map(It.IsAny<EmployerUser>())).Returns(new Person());

            var marketoConfig = new Mock<IOptions<MarketoConfiguration>>();
            marketoConfig.Setup(x => x.Value.ChunkSizeKB).Returns(10000);    // 10MB chunk size
            _chunkingService = new ChunkingService(marketoConfig.Object);

            _handler = new ImportEmployerUsersHandler(_bulkImportServiceMock.Object, _logger.Object,_employerUsersRepositoryMock.Object,_PersonMapperMock.Object);
        }

        [Test]
        public async Task Then_The_Employer_Users_Are_Retrieved()
        {
            //Act
            var status = await _handler.Handle();

            //Assert
            _employerUsersRepositoryMock.Verify(v => v.GetAllUsers(),Times.Once());
        }
        
        [Test]
        public async Task Then_The_List_Is_Sent_To_Marketo()
        {
            var status = await _handler.Handle();

            //Assert
            _bulkImportServiceMock.Verify(s => s.ImportPeople(It.IsAny<IList<Person>>()), Times.Once);
        }

        private static List<EmployerUser> GenerateNewUsers(int leadCount)
        {
            var Leads = Enumerable
                .Range(0, leadCount)
                .Select(i =>
                {
                    var user = new EmployerUser();
                    user.FirstName = $"Firstname{i}";
                    user.LastName = "Surname ";
                    user.Email = $"Firstname{i}.lastname@Email.com";

                    return user;
                })
                .ToList();

            return Leads;
        }
    }
}
