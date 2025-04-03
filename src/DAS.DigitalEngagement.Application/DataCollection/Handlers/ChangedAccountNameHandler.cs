using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class ChangedAccountNameHandler : IChangedAccountNameHandler
    {
        private readonly IChangedAccountNameValidator _validator;
        private readonly IMarketoService _marketoService;
        private readonly IEmployerAccountsRepository _employerAccountsRepository;

        public ChangedAccountNameHandler(IChangedAccountNameValidator validator, IMarketoService marketoService, IEmployerAccountsRepository employerAccountsRepository)
        {
            _validator = validator;
            _employerAccountsRepository = employerAccountsRepository;
            _marketoService = marketoService;
        }

        public async Task Handle(ChangedAccountNameEvent accountName)
        {
            if (!_validator.Validate(accountName))
                throw new ArgumentException("Invalid user data", nameof(accountName));

            var employerUser = await _employerAccountsRepository.GetUserByRef(accountName.UserRef.ToString())
                               ?? throw new ArgumentException("Employer user not found", nameof(accountName));

            if (string.IsNullOrWhiteSpace(employerUser.Email))
                throw new ArgumentException("Employer user email is required", nameof(accountName));

            var userData = new UserData
            {
                EmployerAccountId = accountName.AccountId,
                Email = employerUser.Email,
                StageCompleted = 3,
                StageCompletedText = "Stage 3 - Account Name Confirmed",
                TotalStages = 5,
                DateOfEvent = DateTime.Now
            };

            await _marketoService.PushLead(userData);
        }
    }
}
