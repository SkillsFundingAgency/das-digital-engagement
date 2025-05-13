using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface ICreatedAccountTaskListCompleteValidator
    {
        bool Validate(CreatedAccountTaskListCompleteEvent user);
    }
}
