using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class CreatedAccountTaskListCompleteHandler : ICreatedAccountTaskListCompleteHandler
    {
        private readonly ICreatedAccountTaskListCompleteValidator _validator;
        private readonly IMarketoService _marketoService;
        private readonly IEmployerAccountsRepository _employerAccountsRepository;

        public CreatedAccountTaskListCompleteHandler(ICreatedAccountTaskListCompleteValidator validator, IMarketoService marketoService, IEmployerAccountsRepository employerAccountsRepository)
        {
            _validator = validator;
            _employerAccountsRepository = employerAccountsRepository;
            _marketoService = marketoService;
        }

        public async Task Handle(CreatedAccountTaskListCompleteEvent accountTaskListComplete)
        {
            if (!_validator.Validate(accountTaskListComplete))
                throw new ArgumentException("Invalid user data", nameof(accountTaskListComplete));

            var employerUser = await _employerAccountsRepository.GetUserByRef(accountTaskListComplete.UserRef.ToString())
                               ?? throw new ArgumentException("Employer user not found", nameof(accountTaskListComplete));

            if (string.IsNullOrWhiteSpace(employerUser.Email))
                throw new ArgumentException("Employer user email is required", nameof(employerUser));

            var userData = new UserData
            {
                Email = employerUser.Email,
                StageCompleted = 5,
                StageCompletedText = "Stage 5 - Completed",
                TotalStages = 5,
                DateOfEvent = DateTime.Now
            };

            await _marketoService.PushEmployerRegistrationLead(userData);
        }
    }
}
