using System.Linq;
using DAS.DigitalEngagement.Application.DataCollection.Mapping;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Infrastructure.Models;
using DAS.DigitalEngagement.Models.DataCollection;
using Das.Marketo.RestApiClient.Configuration;
using NUnit.Framework;

namespace DAS.DigitalEngagement.Models.UnitTests
{
    public class PersonMappingWhenMappingEmployerUser
    {
        private PersonMapper _personMapping = new PersonMapper();
        private EmployerUser _employerUser;


        [SetUp]
        public void Arrange()
        {

            _employerUser = new EmployerUser()
            {
                Email = "Test@email.com",
                FirstName = "Firstname",
                LastName = "LastName",
            };
        }

        [Test]
        public void Then_Email_Is_Set()
        {
            //Act
            var actual = _personMapping.Map(_employerUser);

            //Assert
            Assert.AreEqual(_employerUser.Email, actual.Email);
        }

        [Test]
        public void Then_Firstname_Is_Set()
        {
            //Act
            var actual = _personMapping.Map(_employerUser);

            //Assert
            Assert.AreEqual(_employerUser.FirstName, actual.FirstName);
        }

        [Test]
        public void Then_Lastname_Is_Set()
        {
            //Act
            var actual = _personMapping.Map(_employerUser);

            //Assert
            Assert.AreEqual(_employerUser.LastName, actual.LastName);
        }
    }
}