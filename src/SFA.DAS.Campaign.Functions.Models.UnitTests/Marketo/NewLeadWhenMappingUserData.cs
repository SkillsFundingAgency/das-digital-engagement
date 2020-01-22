using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.Marketo;

namespace SFA.DAS.Campaign.Functions.Models.UnitTests.Marketo
{
    public class LeadWhenMappingUserData
    {
        private PushLeadToMarketoRequest _newLead;
        private RegisterInterestProgramConfiguration _regInfoConfig;
        private UserData _userData;


        [SetUp]
        public void Arrange()
        {
            _newLead = new PushLeadToMarketoRequest();

            _userData = new UserData
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@test.com",
                Consent = true,
                RouteId = "1",
                CookieId = "1",
                EncodedEmail = "123ADC"
            };

            _regInfoConfig = new RegisterInterestProgramConfiguration()
            {
                ProgramName = "ProgName",
                Source = "Source",
                LookupField = "Field",
                CitizenReason = "Citizen",
                EmployerReason = "Employer"
            };
        }

        [Test]  
        public void Then_Program_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.ProgramName, actual.ProgramName);
        }
        [Test]
        public void Then_Source_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.Source, actual.Source);
        }
        [Test]
        public void Then_LookupField_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.LookupField, actual.LookupField);
        }
        [Test]
        public void Then_Input_has_one_item()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(1,actual.Input.Count);
        }

        [Test]
        public void Then_First_Input_Firstname_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.FirstName, item.FirstName);
        }
        [Test]
        public void Then_First_Input_Lastname_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.LastName, item.LastName);
        }
        [Test]
        public void Then_First_Input_Email_Is_Set()
        {
            //Act
            var actual = _newLead.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.Email, item.Email);
        }
    }
}