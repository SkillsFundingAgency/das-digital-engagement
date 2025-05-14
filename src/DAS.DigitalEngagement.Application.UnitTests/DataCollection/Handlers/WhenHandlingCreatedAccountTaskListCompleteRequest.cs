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
    public class WhenHandlingCreatedAccountTaskListCompleteRequest
    {
        private CreatedAccountTaskListCompleteHandler _handler;
        private Mock<ICreatedAccountTaskListCompleteValidator> _validator;
        private Mock<IMarketoService> _marketoService;
        private Mock<IEmployerAccountsRepository> _employerAccountsRepository;

        [SetUp]
        public void Arrange()
        {
            _validator = new Mock<ICreatedAccountTaskListCompleteValidator>();
            _marketoService = new Mock<IMarketoService>();
            _employerAccountsRepository = new Mock<IEmployerAccountsRepository>();

            _handler = new CreatedAccountTaskListCompleteHandler(
                _validator.Object,
                _marketoService.Object,
                _employerAccountsRepository.Object
            );
        }

        [Test]
        public void Then_Throws_ArgumentException_If_Validation_Fails()
        {
            var CreatedAccountTaskListComplete = new CreatedAccountTaskListCompleteEvent { UserRef = Guid.NewGuid() };
            _validator.Setup(x => x.Validate(CreatedAccountTaskListComplete)).Returns(false);

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(CreatedAccountTaskListComplete));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void Then_Throws_ArgumentException_If_EmployerUser_Is_Null()
        {
            var CreatedAccountTaskListComplete = new CreatedAccountTaskListCompleteEvent { UserRef = Guid.NewGuid() };
            _validator.Setup(x => x.Validate(CreatedAccountTaskListComplete)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef("abc")).ReturnsAsync((EmployerAccountsUser)null);

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(CreatedAccountTaskListComplete));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void Then_Throws_ArgumentException_If_EmployerUser_Email_Is_Null_Or_Empty()
        {
            var CreatedAccountTaskListComplete = new CreatedAccountTaskListCompleteEvent { UserRef = Guid.NewGuid() };
            _validator.Setup(x => x.Validate(CreatedAccountTaskListComplete)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef("abc")).ReturnsAsync(new EmployerAccountsUser { Email = null });

            Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(CreatedAccountTaskListComplete));
            _marketoService.Verify(x => x.PushLead(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public async Task Then_Creates_AccountTaskListComplete_And_Calls_PushLead_If_Valid()
        {
            var userRef = Guid.NewGuid();
            var CreatedAccountTaskListComplete = new CreatedAccountTaskListCompleteEvent { UserRef = userRef };
            var employerUser = new EmployerAccountsUser { Email = "test@email.com" };

            _validator.Setup(x => x.Validate(CreatedAccountTaskListComplete)).Returns(true);
            _employerAccountsRepository.Setup(x => x.GetUserByRef(userRef.ToString())).ReturnsAsync(employerUser);

            await _handler.Handle(CreatedAccountTaskListComplete);

            _marketoService.Verify(x => x.PushEmployerRegistrationLead(It.Is<UserData>(
                data => data.Email == employerUser.Email &&
                        data.StageCompleted == 5 &&
                        data.StageCompletedText == "Stage 5 - Completed" &&
                        data.TotalStages == 5 &&
                        data.DateOfEvent.Date == DateTime.Now.Date
            )), Times.Once);
        }
    }
}
