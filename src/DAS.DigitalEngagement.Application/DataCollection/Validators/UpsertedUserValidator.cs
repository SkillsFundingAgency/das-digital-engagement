using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class UpsertedUserValidator : IUpsertedUserValidator
    {
        public bool Validate(UpsertedUserEvent userData)
        {

            if (string.IsNullOrWhiteSpace(userData.UserRef))
            {
                return false;
            }

            return true;
        }
    }
}
