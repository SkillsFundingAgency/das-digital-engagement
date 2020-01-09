using System;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Models.UnitTests.DataCollection
{
    public class PersonWhenMappingUserData
    {
        private Person _person;

        [SetUp]
        public void Arrange()
        {
            _person = new Person();
        }

        [Test]
        public void Then_The_Dates_Are_Correctly_Formatted()
        {
            //Arrange
            var expectedDate = "2018-10-20 20:30:25";
            var userData = new UserData
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@test.com",
                Consent = true,
                RouteId = "1",
                CookieId = "1",
                EncodedEmail = "123ADC"
            };

            //Act
            var actual = _person.MapFromUserData(userData,new DateTime(2018, 10, 20, 20, 30, 25));

            //Assert
            Assert.AreEqual(expectedDate, actual.Enrolled);
            Assert.AreEqual(expectedDate, actual.ContactDetail.Captured);
            Assert.AreEqual(expectedDate,actual.Consent.GdprConsentDeclared);
            Assert.AreEqual(expectedDate,actual.Cookie.Captured);
            Assert.AreEqual(expectedDate,actual.Route.Captured);
        }

        [Test]
        public void Then_The_Dates_Are_Not_Set_If_It_Is_Not_A_New_User()
        {
            //Arrange
            var userData = new UserData
            {
                Email = "test@test.com",
                Consent = true
            };

            //Act
            var actual = _person.MapFromUserData(userData);

            //Assert
            Assert.IsNull(actual.Cookie);
            Assert.IsNull(actual.Route);
            Assert.IsNull(actual.Enrolled);
            Assert.IsNull(actual.ContactDetail.Captured);
        }
    }
}