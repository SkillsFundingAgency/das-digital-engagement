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
            Assert.That(actual.ProgramName, Is.EqualTo(_regInfoConfig.ProgramName));
        }
        [Test]
        public void Then_Source_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.That(actual.Source, Is.EqualTo(_regInfoConfig.Source));
        }
        [Test]
        public void Then_LookupField_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.That(actual.LookupField, Is.EqualTo(_regInfoConfig.LookupField));
        }
        [Test]
        public void Then_Input_has_one_item()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            Assert.That(actual.Input.Count, Is.EqualTo(1));
        }

        [Test]
        public void Then_First_Input_Firstname_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.FirstName, Is.EqualTo(_userData.FirstName));
        }
        [Test]
        public void Then_First_Input_Lastname_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.LastName, Is.EqualTo(_userData.LastName));
        }
        [Test]
        public void Then_First_Input_Email_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.Email, Is.EqualTo(_userData.Email));
        }

        [Test]
        public void Then_First_Input_UkEmployerSize_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.UkEmployerSize, Is.EqualTo(_userData.UkEmployerSize));
        }
        [Test]
        public void Then_First_Input_PrimaryIndustry_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.PrimaryIndustry, Is.EqualTo(_userData.PrimaryIndustry));
        }
        [Test]
        public void Then_First_Input_PrimaryLocation_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.PrimaryLocation, Is.EqualTo(_userData.PrimaryLocation));
        }
        [Test]
        public void Then_First_Input_AppsgovSignUpDate_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.AppsgovSignUpDate, Is.EqualTo(_userData.AppsgovSignUpDate));
        }
        [Test]
        public void Then_First_Input_PersonOrigin_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.PersonOrigin, Is.EqualTo(_userData.PersonOrigin));
        }
        [Test]
        public void Then_First_Input_IncludeInUR_Is_Set()
        {
            //Act
            var actual = _userDataMapping.MapFromUserData(_userData, _regInfoConfig);

            //Assert
            var item = actual.Input.First();

            Assert.That(item.IncludeInUR, Is.EqualTo(_userData.IncludeInUR));
        }
    }
}