using System.Threading.Tasks;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Domain.DataCollection
{
    public interface IUserService
    {
        Task RegisterUser(UserData user);

        Task UnregisterUser(UserData user);
    }
}
