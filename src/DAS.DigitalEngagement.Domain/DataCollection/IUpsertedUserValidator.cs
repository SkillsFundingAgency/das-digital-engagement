using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IUpsertedUserValidator
    {
        bool Validate(UpsertedUserEvent user);
    }
}
