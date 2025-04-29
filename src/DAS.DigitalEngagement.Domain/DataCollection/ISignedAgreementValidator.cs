using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface ISignedAgreementValidator
    {
        bool Validate(SignedAgreementEvent payeScheme);
    }
}
