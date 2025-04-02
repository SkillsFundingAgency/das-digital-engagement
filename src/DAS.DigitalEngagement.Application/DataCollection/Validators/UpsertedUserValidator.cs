using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class UpsertedUserValidator : IUpsertedUserValidator
    {
        public bool Validate(UpsertedUserEvent userData)
        {
            if (userData == null) return false;

            return !string.IsNullOrWhiteSpace(userData.UserRef);
        }
    }
}
