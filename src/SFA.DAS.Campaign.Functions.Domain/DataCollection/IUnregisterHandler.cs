using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Domain.DataCollection
{
    public interface IUnregisterHandler
    {
        void Handle(UserData userData);
    }
}