using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Infrastructure.Interfaces.Clients;
using DAS.DigitalEngagement.Models;
using Moq;
using NUnit.Framework;

namespace DAS.DigitalEngagement.Application.UnitTests.Repositories
{
    public class WhenCallingGetUserByRef
    {
        private EmployerAccountsRepository _repository;
        private Mock<IEmployerAccountsApiClient> _apiClientMock;

        [SetUp]
        public void Arrange()
        {
            _apiClientMock = new Mock<IEmployerAccountsApiClient>();
            _repository = new EmployerAccountsRepository(_apiClientMock.Object);
        }

        [Test]
        public async Task Then_Returns_User_If_Found()
        {
            var userRef = "user-123";
            var expectedEmail = "test@example.com";
            var apiUser = new EmployerAccountsUser { Email = expectedEmail };

            _apiClientMock
                .Setup(x => x.GetUserByRef(userRef))
                .ReturnsAsync(apiUser);

            var result = await _repository.GetUserByRef(userRef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(expectedEmail));
            _apiClientMock.Verify(x => x.GetUserByRef(userRef), Times.Once);
        }

        [Test]
        public async Task Then_Returns_Null_If_User_Not_Found()
        {
            var userRef = "user-404";
            _apiClientMock
                .Setup(x => x.GetUserByRef(userRef))
                .ReturnsAsync((EmployerAccountsUser)null);

            var result = await _repository.GetUserByRef(userRef);

            Assert.That(result, Is.Null);
            _apiClientMock.Verify(x => x.GetUserByRef(userRef), Times.Once);
        }
    }
}