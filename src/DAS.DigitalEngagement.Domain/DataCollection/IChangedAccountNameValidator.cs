using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IChangedAccountNameValidator
    {
        bool Validate(ChangedAccountNameEvent accountName);
    }
}
