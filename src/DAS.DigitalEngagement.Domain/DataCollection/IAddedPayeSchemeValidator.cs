using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IAddedPayeSchemeValidator
    {
        bool Validate(AddedPayeSchemeEvent payeScheme);
    }
}
