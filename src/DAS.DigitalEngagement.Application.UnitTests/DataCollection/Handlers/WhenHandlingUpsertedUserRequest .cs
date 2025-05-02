using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.DataCollection.Handlers;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using DAS.DigitalEngagement.Models;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.UnitTests.DataCollection.Handlers
{
    public class WhenHandlingUpsertedUserEvent
    {
        private UpsertedUserHandler _handler;
        private Mock<IUpsertedUserValidator> _validator;
        private Mock<IMarketoService> _marketoService;
        private Mock<IEmployerAccountsRepository> _employerAccountsRepository;

        [SetUp]
        public void Arrange()
        {
            _validator = new Mock<IUpsertedUserValidator>();
            _marketoService = new Mock<IMarketoService>();
            _employerAccountsRepository = new Mock<IEmployerAccountsRepository>();

            _handler = new UpsertedUserHandler(
                _validator.Object,
                _marketoService.Object,
                _employerAccountsRepository.Object
            );
        }

        [Test]
        public void Then_Throws_ArgumentException_If_Validation_Fails()
        {
            var upsertedUser = new UpsertedUserEvent { UserRef = "abc" };
            _validator.Setup(x => x.Validate(upsertedUser)).Returns(false);

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(upsertedUser));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void Then_Throws_ArgumentException_If_EmployerUser_Is_Null()
        {
            var upsertedUser = new UpsertedUserEvent { UserRef = "abc" };
            _validator.Setup(x => x.Validate(upsertedUser)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef("abc")).ReturnsAsync((EmployerAccountsUser)null);

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(upsertedUser));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void Then_Throws_ArgumentException_If_EmployerUser_Email_Is_Null_Or_Empty()
        {
            var upsertedUser = new UpsertedUserEvent { UserRef = "abc" };
            _validator.Setup(x => x.Validate(upsertedUser)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef("abc")).ReturnsAsync(new EmployerAccountsUser { Email = null });

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(upsertedUser));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public async Task Then_Creates_UserData_And_Calls_PushLead_If_Valid()
        {
            var upsertedUser = new UpsertedUserEvent { UserRef = "abc" };
            var employerUser = new EmployerAccountsUser { Email = "test@email.com" };

            _validator.Setup(x => x.Validate(upsertedUser)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef("abc")).ReturnsAsync(employerUser);

            await _handler.Handle(upsertedUser);

            _marketoService.Verify(x => x.PushEmployerRegistrationLead(It.Is<UserData>(
                data => data.Email == employerUser.Email &&
                        data.StageCompleted == 1 &&
                        data.StageCompletedText == "Stage 1 - User details Completed" &&
                        data.TotalStages == 5 &&
                        data.DateOfEvent.Date == DateTime.Now.Date
            )), Times.Once);
        }
    }
}
