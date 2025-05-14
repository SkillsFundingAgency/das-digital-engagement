using DAS.DigitalEngagement.Domain.DataCollection;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Application.DataCollection.Validators
{
    public class CreatedAccountTaskListCompleteValidator : ICreatedAccountTaskListCompleteValidator
    {
        public bool Validate(CreatedAccountTaskListCompleteEvent userData)
        {
            if (userData == null) return false;

            return !string.IsNullOrWhiteSpace(userData.UserRef.ToString());
        }
    }
}
