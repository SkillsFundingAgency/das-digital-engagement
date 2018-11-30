using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.UnitTests.DataCollection.Handlers
{
    public class WhenHandlingStoreDataRequest
    {
        private RegisterHandler _handler;
        private Mock<IUserDataValidator> _validator;
        private Mock<IUserService> _userService;
        private Mock<IWiredPlusService> _wiredPlusService;

        [SetUp]
        public void Arrange()
        {
            _validator = new Mock<IUserDataValidator>();
            _userService = new Mock<IUserService>();
            _wiredPlusService = new Mock<IWiredPlusService>();

            _validator.Setup(x => x.Validate(It.IsAny<UserData>())).Returns(true);
            _handler = new RegisterHandler(_validator.Object, _userService.Object, _wiredPlusService.Object);
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
            _userService.Verify(x=>x.RegisterUser(It.IsAny<UserData>()), Times.Never);
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
            _userService.Verify(x => x.RegisterUser(It.Is<UserData>(c=>c.Equals(expectedUserData))), Times.Once);
        }

        [Test]
        public async Task Then_If_The_Message_Is_Valid_Is_Sent_To_The_WiredPlus_Api()
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
            _wiredPlusService.Verify(x => x.CreateUser(It.Is<UserData>(c => c.Equals(expectedUserData))), Times.Once);
        }


        [Test]
        public async Task Then_If_The_Message_Is_Valid_Is_Sent_To_The_WiredPlus_Api_To_Subscribe()
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
            _wiredPlusService.Verify(x => x.SubscribeUser(It.Is<UserData>(c => c.Equals(expectedUserData))), Times.Once);
        }
    }
}
