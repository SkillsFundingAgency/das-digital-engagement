using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection.Services.UserService
{
    public class WhenStoringUserData
    {
        private Application.DataCollection.Services.UserService _userService;
        private Mock<IHttpClient<Person>> _httpClient;
        private Mock<IOptions<Configuration>> _configuration;
        private UserData _userData;

        [SetUp]
        public void Arrange()
        {
            _userData = new UserData
            {
                Consent = true,
                RouteId = "1",
                CookieId = "23",
                FirstName = "Test",
                LastName = "Tester",
                Email = "test@tester.com"
            };
            _httpClient = new Mock<IHttpClient<Person>>();
            _configuration = new Mock<IOptions<Configuration>>();
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Person>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Accepted));
            _configuration.Setup(x => x.Value.ApiBaseUrl).Returns("http://test.local/api");

            _userService = new Application.DataCollection.Services.UserService(_httpClient.Object, _configuration.Object);
        }

        [Test]
        public async Task Then_The_Auth_Key_Is_Set()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _userService.RegisterUser(expectedUser);

            //Assert
            _configuration.Verify(c => c.Value.ApiXFunctionsKey, Times.Once);
        }
        [Test]
        public async Task Then_The_User_Data_Service_Is_Called_With_The_Passed_In_Model()
        {
            
            //Act
            await _userService.RegisterUser(_userData);

            //Assert
            _httpClient.Verify(x => x.PostAsync("http://test.local/api/create-person", 
                It.Is<Person>(p=>
                    p.Consent.GdprConsentGiven.Equals(_userData.Consent) && 
                    p.ContactDetail.EmailAddress.Equals(_userData.Email) &&
                    p.Cookie.CookieIdentifier.Equals(_userData.CookieId) &&
                    p.FirstName.Equals(_userData.FirstName) &&
                    p.LastName.Equals(_userData.LastName) &&
                    p.Route.RouteIdentifier.Equals(_userData.RouteId) 
            )));
            _httpClient.Verify(x => x.PostAsync("http://test.local/api/update-person", It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Then_If_The_Request_Is_Rejected_A_InvalidOperationException_Is_Thrown()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<Person>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.RegisterUser(new UserData()));
        }

        [Test]
        public async Task Then_If_A_Conflict_Is_Returned_The_Update_Endpoint_Is_Called()
        {
            //Arrange
            _httpClient.Setup(x => x.PostAsync("http://test.local/api/create-person", It.IsAny<Person>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Conflict));
            _httpClient.Setup(x => x.PostAsync("http://test.local/api/update-person", It.IsAny<Person>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Accepted));

            //Act
            await _userService.RegisterUser(_userData);

            //Act
            _httpClient.Verify(x => x.PostAsync("http://test.local/api/create-person",
                It.Is<Person>(p =>
                    p.Consent.GdprConsentGiven.Equals(_userData.Consent) &&
                    p.ContactDetail.EmailAddress.Equals(_userData.Email) &&
                    p.Cookie.CookieIdentifier.Equals(_userData.CookieId) &&
                    p.FirstName.Equals(_userData.FirstName) &&
                    p.LastName.Equals(_userData.LastName) &&
                    p.Route.RouteIdentifier.Equals(_userData.RouteId)
                )), Times.Once);
            _httpClient.Verify(x => x.PostAsync("http://test.local/api/update-person",
                It.Is<Person>(p =>
                    p.Consent.GdprConsentGiven.Equals(_userData.Consent) &&
                    p.ContactDetail.EmailAddress.Equals(_userData.Email) &&
                    p.Cookie.CookieIdentifier.Equals(_userData.CookieId) &&
                    p.FirstName.Equals(_userData.FirstName) &&
                    p.LastName.Equals(_userData.LastName) &&
                    p.Route.RouteIdentifier.Equals(_userData.RouteId)
                )), Times.Once);
        }
    }
}