using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Application.Services;
using Services = SFA.DAS.Campaign.Functions.Application.DataCollection.Services;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.Marketo;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection.Services.MarketoLeadService
{
    public class WhenStoringUserData
    {
        private Application.DataCollection.Services.MarketoLeadService _marketoLeadService;
        private Mock<IMarketoLeadClient> _MarketoLeadClient;
        private Mock<IOptions<MarketoConfiguration>> _configuration;
        private UserData _employerUserData;
        private UserData _citizenUserData;

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
                Email = "test@tester.com"
            };
            _employerUserData = new UserData
            {
                Consent = true,
                RouteId = "2",
                CookieId = "23",
                FirstName = "Test",
                LastName = "Tester",
                Email = "test@tester.com"
            };
            _MarketoLeadClient = new Mock<IMarketoLeadClient>();
            _configuration = new Mock<IOptions<MarketoConfiguration>>();
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.ProgramName).Returns("ProgrammeName");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.Source).Returns("Source");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.LookupField).Returns("LookupField");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.CitizenReason).Returns("CitizenReason");
            _configuration.Setup(x => x.Value.RegisterInterestProgramConfiguration.EmployerReason).Returns("EmployerReason");

            //Arrange
            _MarketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
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

            _MarketoLeadClient.Setup(s => s.AssociateLead(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ResponseWithoutResult(success: true, requestId: "") { Errors = new List<Error>() });


            _marketoLeadService = new Application.DataCollection.Services.MarketoLeadService(_MarketoLeadClient.Object, _configuration.Object);
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
            _MarketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
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
            _MarketoLeadClient.Verify(x => x.PushLead(It.IsAny<PushLeadToMarketoRequest>()), Times.Once);
            _MarketoLeadClient.Verify(v => v.AssociateLead(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

        }

        [Test]
        public void Then_If_The_PushLead_Request_Is_Rejected_A_Exception_Is_Thrown()
        {
            //Arrange
            _MarketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
                .ReturnsAsync(new ResponseOfPushLeadToMarketo(errors: new List<Error>(), requestId: "") { Success = false });

            //Act Assert
            Assert.ThrowsAsync<Exception>(async () => await _marketoLeadService.PushLead(new UserData()));
        }

        [Test]
        public void Then_If_The_AssociateLead_Request_Is_Rejected_A_Exception_Is_Thrown()
        {
            //Arrange
            _MarketoLeadClient.Setup(s => s.PushLead(It.IsAny<PushLeadToMarketoRequest>()))
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
            _MarketoLeadClient.Setup(v => v.AssociateLead(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ResponseWithoutResult(errors: new List<Error>(), requestId: "") { Success = false, Errors = new List<Error>() });
            //Act Assert
            Assert.ThrowsAsync<Exception>(async () => await _marketoLeadService.PushLead(new UserData()));
        }

    }
}