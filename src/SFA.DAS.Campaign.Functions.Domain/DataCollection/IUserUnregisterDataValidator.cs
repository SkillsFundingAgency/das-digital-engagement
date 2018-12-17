namespace SFA.DAS.Campaign.Functions.Domain.DataCollection
{
    public interface IUserUnregisterDataValidator
    {
        bool Validate(string email);
    }
}