using SFA.DAS.EmployerAccounts.Messages.Events;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IUpsertedUserHandler
    {
        Task Handle(UpsertedUserEvent userData);
    }
}
