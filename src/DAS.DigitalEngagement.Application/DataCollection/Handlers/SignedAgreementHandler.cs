using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class SignedAgreementHandler : ISignedAgreementHandler
    {
        private readonly ISignedAgreementValidator _validator;
        private readonly IMarketoService _marketoService;
        private readonly IEmployerAccountsRepository _employerAccountsRepository;

        public SignedAgreementHandler(ISignedAgreementValidator validator, IMarketoService marketoService, IEmployerAccountsRepository employerAccountsRepository)
        {
            _validator = validator;
            _employerAccountsRepository = employerAccountsRepository;
            _marketoService = marketoService;
        }

        public async Task Handle(SignedAgreementEvent signedAgreement)
        {
            if (!_validator.Validate(signedAgreement))
                throw new ArgumentException("Invalid user data", nameof(signedAgreement));

            var employerUser = await _employerAccountsRepository.GetUserByRef(signedAgreement.UserRef.ToString())
                               ?? throw new ArgumentException("Employer user not found", nameof(signedAgreement));

            if (string.IsNullOrWhiteSpace(employerUser.Email))
                throw new ArgumentException("Employer user email is required", nameof(signedAgreement));

            var userData = new UserData
            {
                EmployerAccountId = signedAgreement.AccountId,
                Email = employerUser.Email,
                StageCompleted = 4,
                StageCompletedText = "Stage 4 - Employer Agreement Signed",
                TotalStages = 5,
                DateOfEvent = DateTime.Now
            };

            await _marketoService.PushEmployerRegistrationLead(userData);
        }
    }
}
