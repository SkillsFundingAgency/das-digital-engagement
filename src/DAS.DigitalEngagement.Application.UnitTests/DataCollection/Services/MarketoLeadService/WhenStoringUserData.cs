using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using DAS.DigitalEngagement.Models.DataCollection;
using Das.Marketo.RestApiClient.Configuration;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Application.UnitTests.DataCollection.Services.MarketoLeadService
{
    public class WhenStoringUserData
    {
        private Application.DataCollection.Services.MarketoLeadService _marketoLeadService;
        private Mock<IMarketoLeadClient> _marketoLeadClient;
        private Mock<IOptions<MarketoConfiguration>> _configuration;
        private UserData _employerUserData;
        private UserData _citizenUserData;
        private string _marketoCookieID = "TestCookieId";

        [SetUp]
        public void Arrange()
        {
            _citizenUserData = new UserData
            {
                Consent = true,
                RouteId = "1",
                CookieId = "23",
                FirstName = "Test",
                LastName = "Tester",
                Email = "test@tester.com",
                MarketoCookieId = _marketoCookieID
            };
            _employerUserData = new UserData
            {
                Consent = true,
                RouteId = "2",
                CookieId = "23",
                FirstName = "Test",
                LastName = "Tester",
                Email = "test@tester.com",
                MarketoCookieId = _marketoCookieID
            };
            _marketoLeadClient = new Mock<IMarketoLeadClient>();
            _configuration = new Mock<IOptions<MarketoConfiguration>>();
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.ProgramName).Returns("ProgrammeName");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.Source).Returns("Source");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.LookupField).Returns("LookupField");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.CitizenReason).Returns("CitizenReason");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.EmployerReason).Returns("EmployerReason");

            //Arrange
            _marketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo(success: true)
                {
                    Result = new List<Lead>()
                            {
                                new Lead()
                                {
                                    Id = 123
                                }
                            }
                });

            _marketoLeadClient.Setup(s => s.AssociateLead(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ResponseWithoutResult(success: true, requestId: "") { Errors = new List<Error>() });


            _marketoLeadService = new Application.DataCollection.Services.MarketoLeadService(_marketoLeadClient.Object, _configuration.Object);
        }

        [Test]
        public async Task Then_The_ProgramName_Is_Set()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _marketoLeadService.PushLead(expectedUser);

            //Assert
            _configuration.Verify(c => c.Value.RegisterInterestProgramConfiguration.ProgramName, Times.Once);
        }
        [Test]
        public async Task Then_The_Source_Is_Set()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _marketoLeadService.PushLead(expectedUser);

            //Assert
            _configuration.Verify(c => c.Value.RegisterInterestProgramConfiguration.Source, Times.Once);
        }

        [Test]
        public async Task Then_The_LookupField_Is_Set()
        {
            //Arrange
            var expectedUser = new UserData();

            //Act
            await _marketoLeadService.PushLead(expectedUser);

            //Assert
            _configuration.Verify(c => c.Value.RegisterInterestProgramConfiguration.LookupField, Times.Once);
        }

        [Test]
        public async Task Then_The_CitizenReason_Is_Set()
        {
            //Act
            await _marketoLeadService.PushLead(_citizenUserData);

            //Assert
            _configuration.Verify(c => c.Value.RegisterInterestProgramConfiguration.CitizenReason, Times.Once);
        }
        [Test]
        public async Task Then_The_EmployerReason_Is_Set()
        {
            //Act
            await _marketoLeadService.PushLead(_employerUserData);

            //Assert
            _configuration.Verify(c => c.Value.RegisterInterestProgramConfiguration.EmployerReason, Times.Once);
        }
        [Test]
        public async Task Then_The_Marketo_lead_Service_Is_Called()
        {
            //Arrange
            _marketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo()
                {
                    Success = true,
                    Errors = new List<Error>(),
                    Result = new List<Lead>()
                        {
                            new Lead()
                            {
                                Id = 123
                            }
                        }

                });

            //Act
            await _marketoLeadService.PushLead(_employerUserData);

            //Assert
            _marketoLeadClient.Verify(x => x.PushLead(It.IsAny<PushLeadToMarketoRequest>()), Times.Once);
            _marketoLeadClient.Verify(v => v.AssociateLead(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_MarketoCookiId_has_no_Value_Then_The_Marketo_AssociateLead_Is_Not_Called(string cookieId)
        {
            _employerUserData.MarketoCookieId = cookieId;
            //Arrange
            _marketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo()
                {
                    Success = true,
                    Errors = new List<Error>(),
                    Result = new List<Lead>()
                    {
                        new Lead()
                        {
                            Id = 123
                        }
                    }

                });

            //Act
            await _marketoLeadService.PushLead(_employerUserData);

            //Assert
            _marketoLeadClient.Verify(x => x.PushLead(It.IsAny<PushLeadToMarketoRequest>()), Times.Once);
            _marketoLeadClient.Verify(v => v.AssociateLead(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

            _employerUserData.MarketoCookieId = _marketoCookieID;
        }

        [Test]
        public void Then_If_The_PushLead_Request_Is_Rejected_A_Exception_Is_Thrown()
        {
            //Arrange
            _marketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo(errors: new List<Error>(), requestId: "") { Success = false });

            //Act Assert
            Assert.ThrowsAsync<Exception>(async () => await _marketoLeadService.PushLead(new UserData()));
        }

        [Test]
        public void Then_If_The_AssociateLead_Request_Is_Rejected_A_Exception_Is_Thrown()
        {
            //Arrange
            _marketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo(errors: new List<Error>(), requestId: "")
                {
                    Success = true,
                    Result = new List<Lead>()
                {
                    new Lead()
                    {
                        Id = 123
                    }

                }
                });
            _marketoLeadClient.Setup(v => v.AssociateLead(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ResponseWithoutResult(errors: new List<Error>(), requestId: "") { Success = false, Errors = new List<Error>() });
            //Act Assert
            Assert.ThrowsAsync<Exception>(async () => await _marketoLeadService.PushLead(_employerUserData));
        }

    }
}