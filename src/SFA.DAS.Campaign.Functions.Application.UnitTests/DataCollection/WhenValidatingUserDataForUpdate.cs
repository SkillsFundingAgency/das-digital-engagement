using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Validators;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection
{
    public class WhenValidatingUserDataForUpdate
    {
        private UserUnregisterDataValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new UserUnregisterDataValidator();
        }

        [Test]
        public void Then_If_There_Is_No_Value_For_Email_Validation_Fails()
        {
            //Act
            var actual = _validator.Validate(string.Empty);

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void Then_True_Is_Returned_If_The_Model_Is_Correctly_Populated()
        {
            //Act
            var actual = _validator.Validate("a'a@a.com");

            //Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void Then_False_Is_Returned_If_The_Email_Is_Not_Valid()
        {
            //Act
            var actual = _validator.Validate("a");

            //Assert
            Assert.IsFalse(actual);
        }
    }
}
