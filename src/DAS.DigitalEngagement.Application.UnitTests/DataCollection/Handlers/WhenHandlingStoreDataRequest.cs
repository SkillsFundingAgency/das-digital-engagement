using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.DataCollection.Handlers;
using Moq;
using NUnit.Framework;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Application.UnitTests.DataCollection.Handlers
{
    public class WhenHandlingStoreDataRequest
    {
        private RegisterHandler _handler;
        private Mock<IUserDataValidator> _validator;
        private Mock<IMarketoService> _marketoService;

        [SetUp]
        public void Arrange()
        {
            _validator = new Mock<IUserDataValidator>();
            _marketoService = new Mock<IMarketoService>();

            _validator.Setup(x => x.Validate(It.IsAny<UserData>())).Returns(true);
            _handler = new RegisterHandler(_validator.Object,_marketoService.Object);
        }

        [Test]
        public void Then_The_Message_Is_Validated_And_ArgumentException_Thrown_And_Not_Sent_To_The_Api_If_Not_Valid()
        {
            //Arrange
            _validator.Setup(x => x.Validate(It.IsAny<UserData>())).Returns(false);

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(new UserData()));

            //Assert
            _validator.Verify(x=>x.Validate(It.IsAny<UserData>()), Times.Once);
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()),Times.Never);
        }

        [Test]
        public async Task Then_If_The_Message_Is_Valid_Is_Sent_To_The_Api()
        {
            //Arrange
            var expectedUserData = new UserData
            {
                Consent = true,
                CookieId = "123",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Tester",
                RouteId = "1"
            };

            //Act
            await _handler.Handle(expectedUserData);

            //Assert
            _marketoService.Verify(x => x.PushLead(It.Is<UserData>(c => c.Equals(expectedUserData))), Times.Once);
        }

        [Test]
        public async Task Then_If_The_Message_Is_Valid_Is_Sent_To_The_MarketoService()
        {
            //Arrange
            var expectedUserData = new UserData
            {
                Consent = true,
                CookieId = "123",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Tester",
                RouteId = "1"
            };

            //Act
            await _handler.Handle(expectedUserData);

            //Assert
            _marketoService.Verify(x => x.PushLead(It.Is<UserData>(c => c.Equals(expectedUserData))), Times.Once);
        }
    }
}
