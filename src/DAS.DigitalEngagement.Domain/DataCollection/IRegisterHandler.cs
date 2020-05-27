using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IRegisterHandler
    {
        Task Handle(UserData userData);
    }
}