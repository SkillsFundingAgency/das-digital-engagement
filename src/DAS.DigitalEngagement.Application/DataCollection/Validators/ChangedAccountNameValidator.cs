using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;
using System;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class ChangedAccountNameValidator : IChangedAccountNameValidator
    {
        public bool Validate(ChangedAccountNameEvent accountName)
        {
            if (accountName == null) return false;

            return accountName.UserRef != Guid.Empty;
        }
    }
}
