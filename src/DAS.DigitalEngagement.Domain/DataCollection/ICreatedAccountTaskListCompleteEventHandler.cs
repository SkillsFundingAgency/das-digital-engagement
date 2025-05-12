using SFA.DAS.EmployerAccounts.Messages.Events;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface ICreatedAccountTaskListCompleteEventHandler
    {
        Task Handle(CreatedAccountTaskListCompleteEvent userData);
    }
}
