using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Services;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection.Services
{
    public class WhenCheckingIfAUserExistsByEmailInWiredPlus
    {
        private WiredPlusService _wiredPlusService;
        private Mock<IHttpClient<Dictionary<string, string>>> _httpClient;
        private Mock<IOptions<Configuration>> _configuration;
        private const string ExpectedAuthKey = "testauth";
        private const string ExpectedUserEmail = "test@local.com";
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
        public async Task Then_The_Email_Is_Converted_To_A_Dictionary()
        {
            //Act
            await _wiredPlusService.UserExists(ExpectedUserEmail);

            //Assert
            _httpClient.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<Dictionary<string, string>>(
                c => c.ContainsKey("email")
                )), Times.Once);
        }

        [Test]
        public async Task Then_The_Request_Is_Sent_To_The_Get_User_Endpoint()
        {
            //Act
            await _wiredPlusService.UserExists(ExpectedUserEmail);

            _httpClient.Verify(x => x.PostAsync(It.Is<string>(c => c.Equals($"{ExpecteBaseUrl}/v1/GetContactByEmail", StringComparison.CurrentCultureIgnoreCase)),
                It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public async Task Then_The_Auth_Key_Is_Set()
        {
            //Act
            await _wiredPlusService.UserExists(ExpectedUserEmail);

            //Assert
            _configuration.Verify(c => c.Value.WiredPlusAuthKey, Times.Once);
        }


        [Test]
        public async Task Then_The_Data_Is_Sent_To_The_Api()
        {
            //Act
            await _wiredPlusService.UserExists(ExpectedUserEmail);

            //Assert
            _httpClient.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<Dictionary<string, string>>(
                c => c.ContainsValue(ExpectedUserEmail))), Times.Once);
        }
        
        [Test]
        public async Task Then_False_Is_Returned_If_The_User_Does_Not_Exist()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            //Act
            var actual =  await _wiredPlusService.UserExists(ExpectedUserEmail);

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task Then_True_Is_Returned_If_The_User_Does_Exist()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Found));

            //Act
            var actual = await _wiredPlusService.UserExists(ExpectedUserEmail);

            //Assert
            Assert.IsTrue(actual);
        }
    }
}
