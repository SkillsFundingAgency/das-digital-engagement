using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Services;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection.Services
{
    public class WhenSubscribingUserDataInWiredPlusApi
    {
        private WiredPlusService _wiredPlusService;
        private Mock<IHttpClient<Dictionary<string, string>>> _httpClient;
        private Mock<IOptions<Configuration>> _configuration;
        private const string ExpectedAuthKey = "testauth";
        private const string ExpecteBaseUrl = "https://test.wiredlocal";

        [SetUp]
        public void Arrange()
        {
            _httpClient = new Mock<IHttpClient<Dictionary<string, string>>>();
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Accepted));
            _httpClient.Setup(x => x.AuthKey).Verifiable();

            _configuration = new Mock<IOptions<Configuration>>();
            _configuration.Setup(x => x.Value.WiredPlusBaseUrl).Returns(ExpecteBaseUrl);
            _configuration.Setup(x => x.Value.WiredPlusAuthKey).Returns(ExpectedAuthKey);

            _wiredPlusService = new WiredPlusService(_httpClient.Object, _configuration.Object);
        }

        [Test]
        public async Task Then_The_User_Object_Is_Converted_To_A_Dictionary()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _wiredPlusService.SubscribeUser(expectedUser);

            //Assert
            _httpClient.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<Dictionary<string, string>>(
                c => c.ContainsKey("email")
                )), Times.Once);
        }

        [Test]
        public async Task Then_The_Request_Is_Sent_To_The_Create_User_Endpoint()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _wiredPlusService.SubscribeUser(expectedUser);

            _httpClient.Verify(x => x.PostAsync(It.Is<string>(c => c.Equals($"{ExpecteBaseUrl}/v1/ResubscribeContact", StringComparison.CurrentCultureIgnoreCase)),
                It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public async Task Then_The_Auth_Key_Is_Set()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _wiredPlusService.SubscribeUser(expectedUser);

            //Assert
            _configuration.Verify(c => c.Value.WiredPlusAuthKey, Times.Once);
        }


        [Test]
        public async Task Then_The_Data_Is_Sent_The_Api()
        {
            //Arrange
            var expectedUser = new UserData { Email = "test@test.com", FirstName = "Test", LastName = "Tester" };

            //Act
            await _wiredPlusService.SubscribeUser(expectedUser);

            //Assert
            _httpClient.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<Dictionary<string, string>>(
                c => c.ContainsValue("test@test.com")
            )), Times.Once);
        }

        [Test]
        public void Then_An_Invalid_Operation_Exception_Is_Thrown_If_The_Response_Does_Not_Indicate_Success()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _wiredPlusService.SubscribeUser(new UserData()));
        }
    }
}
