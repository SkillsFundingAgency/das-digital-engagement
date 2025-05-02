using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class UpsertedUserHandler : IUpsertedUserHandler
    {
        private readonly IUpsertedUserValidator _validator;
        private readonly IMarketoService _marketoService;
        private readonly IEmployerAccountsRepository _employerAccountsRepository;

        public UpsertedUserHandler(IUpsertedUserValidator validator, IMarketoService marketoService, IEmployerAccountsRepository employerAccountsRepository)
        {
            _validator = validator;
            _employerAccountsRepository = employerAccountsRepository;
            _marketoService = marketoService;
        }

        public async Task Handle(UpsertedUserEvent upsertedUser)
        {
            if (!_validator.Validate(upsertedUser))
                throw new ArgumentException("Invalid user data", nameof(upsertedUser));

            var employerUser = await _employerAccountsRepository.GetUserByRef(upsertedUser.UserRef)
                               ?? throw new ArgumentException("Employer user not found", nameof(upsertedUser));

            if (string.IsNullOrWhiteSpace(employerUser.Email))
                throw new ArgumentException("Employer user email is required", nameof(employerUser));

            var userData = new UserData
            {
                Email = employerUser.Email,
                StageCompleted = 1,
                StageCompletedText = "Stage 1 - User details Completed",
                TotalStages = 5,
                DateOfEvent = DateTime.Now
            };

            await _marketoService.PushEmployerRegistrationLead(userData);
        }
    }
}
