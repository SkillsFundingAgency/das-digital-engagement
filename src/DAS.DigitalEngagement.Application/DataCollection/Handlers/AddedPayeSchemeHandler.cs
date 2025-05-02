using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class AddedPayeSchemeHandler : IAddedPayeSchemeHandler
    {
        private readonly IAddedPayeSchemeValidator _validator;
        private readonly IMarketoService _marketoService;
        private readonly IEmployerAccountsRepository _employerAccountsRepository;

        public AddedPayeSchemeHandler(IAddedPayeSchemeValidator validator, IMarketoService marketoService, IEmployerAccountsRepository employerAccountsRepository)
        {
            _validator = validator;
            _employerAccountsRepository = employerAccountsRepository;
            _marketoService = marketoService;
        }

        public async Task Handle(AddedPayeSchemeEvent payeScheme)
        {
            if (!_validator.Validate(payeScheme))
                throw new ArgumentException("Invalid user data", nameof(payeScheme));

            var employerUser = await _employerAccountsRepository.GetUserByRef(payeScheme.UserRef.ToString())
                               ?? throw new ArgumentException("Employer user not found", nameof(payeScheme));

            if (string.IsNullOrWhiteSpace(employerUser.Email))
                throw new ArgumentException("Employer user email is required", nameof(payeScheme));

            var userData = new UserData
            {
                EmployerAccountId = payeScheme.AccountId,
                Email = employerUser.Email,
                StageCompleted = 2,
                StageCompletedText = "Stage 2 - PAYE Added",
                TotalStages = 5,
                DateOfEvent = DateTime.Now
            };

            await _marketoService.PushEmployerRegistrationLead(userData);
        }
    }
}
