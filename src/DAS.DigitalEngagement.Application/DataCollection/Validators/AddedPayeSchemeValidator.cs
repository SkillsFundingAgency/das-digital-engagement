using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;
using System;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class AddedPayeSchemeValidator : IAddedPayeSchemeValidator
    {
        public bool Validate(AddedPayeSchemeEvent payeScheme)
        {
            if (payeScheme == null) return false;

            return payeScheme.UserRef != Guid.Empty;
        }
    }
}
