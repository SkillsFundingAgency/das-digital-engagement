using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IMarketoService
    {
        Task PushLead(UserData user);
        Task PushEmployerRegistrationLead(UserData user);
    }
}
