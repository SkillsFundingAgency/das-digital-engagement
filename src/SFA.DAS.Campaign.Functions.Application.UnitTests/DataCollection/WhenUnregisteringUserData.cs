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

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection
{
    public class WhenUnregisteringUserData
    {
        private UserService _userService;
        private Mock<IHttpClient<UserData>> _httpClient;
        private Mock<IOptions<Configuration>> _configuration;

        [SetUp]
        public void Arrange()
        {
            _httpClient = new Mock<IHttpClient<UserData>>();
            _configuration = new Mock<IOptions<Configuration>>();
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<UserData>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Accepted));
            _configuration.Setup(x => x.Value.ApiBaseUrl).Returns("http://test.local/api");

            _userService = new UserService(_httpClient.Object, _configuration.Object);
        }

        [Test]
        public async Task Then_The_User_Data_Service_Is_Called_With_The_Passed_In_Model()
        {
            //Arrange
            var userData = new UserData
            {
                Consent = true,
                RouteId = "1",
                CookieId = "23",
                FirstName = "Test",
                LastName = "Tester",
                Email = "test@tester.com"
            };

            //Act
            await _userService.UnregisterUser(userData);

            //Assert
            _httpClient.Verify(x => x.PostAsync("http://test.local/api/unregisterdetails", userData));
        }

        [Test]
        public void Then_If_The_Request_Is_Rejected_A_InvalidOperationException_Is_Thrown()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<UserData>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.UnregisterUser(new UserData()));
        }
    }
}
