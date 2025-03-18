using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DAS.DigitalEngagement.Application.DataCollection.Validators;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Application.UnitTests.DataCollection
{
    public class WhenValidatingUserData
    {
        private UserDataValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new UserDataValidator();
        }

        [TestCase("test@test.com", "a", "b", "1", "")]
        [TestCase("test@test.com", "a", "b", "", "2")]
        [TestCase("test@test.com", "a", "", "1", "2")]
        [TestCase("test@test.com", "", "b", "1", "2")]
        [TestCase("", "a", "b", "1", "2")]
        [TestCase(" ", " ", " ", " ", " ")]
        [TestCase("", "", "", "", "")]
        public void Then_If_There_Are_Missing_Values_False_Is_Returned(string email, string firstName, string lastName, string routeId, string cookieId)
        {
            //Act
            var actual = _validator.Validate(new UserData());

            //Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void Then_True_Is_Returned_If_The_Model_Is_Correctly_Populated()
        {
            //Act
            var actual = _validator.Validate(new UserData
            {
                Email = "a'a@a.com",
                FirstName = "test",
                LastName = "tester",
                RouteId = "123",
                CookieId = "54321"
            });

            //Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Then_False_Is_Returned_If_The_Email_Is_Not_Valid()
        {
            //Act
            var actual = _validator.Validate(new UserData
            {
                Email = "a",
                FirstName = "test",
                LastName = "tester",
                RouteId = "123",
                CookieId = "54321"
            });

            //Assert
            Assert.That(actual, Is.False);
        }
    }
}
