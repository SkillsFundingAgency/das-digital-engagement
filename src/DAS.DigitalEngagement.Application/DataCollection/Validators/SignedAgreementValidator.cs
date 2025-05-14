using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;
using System;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class SignedAgreementValidator : ISignedAgreementValidator
    {
        public bool Validate(SignedAgreementEvent signedAgreement)
        {
            if (signedAgreement == null) return false;

            return signedAgreement.UserRef != Guid.Empty;
        }
    }
}
