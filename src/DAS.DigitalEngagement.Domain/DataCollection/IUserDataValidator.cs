using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IUserDataValidator
    {
        bool Validate(UserData user);
    }
}
