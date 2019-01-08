using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Validators
{
    public class UserUnregisterDataValidator : IUserUnregisterDataValidator
    {
        public bool Validate(string email)
        {
            var userData = new UserData{Email = email};
            return userData.IsValidEmail();
        }
    }
}