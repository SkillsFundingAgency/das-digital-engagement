using System;
using System.Linq;
using DAS.DigitalEngagement.Application.DataCollection.Mapping;
using DAS.DigitalEngagement.Models.DataCollection;
using Das.Marketo.RestApiClient.Configuration;
using Das.Marketo.RestApiClient.Models;
using NUnit.Framework;

namespace DAS.DigitalEngagement.Models.UnitTests.Marketo
{
    public class LeadWhenMappingUserData
    {
        private RegisterInterestProgramConfiguration _regInfoConfig;
        private UserDataMapping _userDataMapping = new UserDataMapping();
        private UserData _userData;


        [SetUp]
        public void Arrange()
        {

            _userData = new UserData
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@test.com",
                UkEmployerSize = "testSize",
                PrimaryIndustry = "testIndustry",
                PrimaryLocation = "testLocation",
                AppsgovSignUpDate = DateTime.Now,
                PersonOrigin = "testOrigin",
                Consent = true,
                RouteId = "1",
                CookieId = "1",
                EncodedEmail = "123ADC",
                IncludeInUR = true
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
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.ProgramName, actual.ProgramName);
        }
        [Test]
        public void Then_Source_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.Source, actual.Source);
        }
        [Test]
        public void Then_LookupField_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(_regInfoConfig.LookupField, actual.LookupField);
        }
        [Test]
        public void Then_Input_has_one_item()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.AreEqual(1,actual.Input.Count);
        }

        [Test]
        public void Then_First_Input_Firstname_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.FirstName, item.FirstName);
        }
        [Test]
        public void Then_First_Input_Lastname_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.LastName, item.LastName);
        }
        [Test]
        public void Then_First_Input_Email_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.Email, item.Email);
        }

        [Test]
        public void Then_First_Input_UkEmployerSize_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.UkEmployerSize, item.UkEmployerSize);
        }
        [Test]
        public void Then_First_Input_PrimaryIndustry_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.PrimaryIndustry, item.PrimaryIndustry);
        }
        [Test]
        public void Then_First_Input_PrimaryLocation_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.PrimaryLocation, item.PrimaryLocation);
        }
        [Test]
        public void Then_First_Input_AppsgovSignUpDate_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.AppsgovSignUpDate, item.AppsgovSignUpDate);
        }
        [Test]
        public void Then_First_Input_PersonOrigin_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.PersonOrigin, item.PersonOrigin);
        }
        [Test]
        public void Then_First_Input_IncludeInUR_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.AreEqual(_userData.IncludeInUR, item.IncludeInUR);
        }
    }
}