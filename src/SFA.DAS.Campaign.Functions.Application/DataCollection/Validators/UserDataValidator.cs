using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Validators
{
    public class UserDataValidator : IUserDataValidator
    {
        public bool Validate(UserData userData)
        {

            if (string.IsNullOrWhiteSpace(userData.FirstName) ||
                string.IsNullOrWhiteSpace(userData.LastName) ||
                string.IsNullOrWhiteSpace(userData.Email) ||
                string.IsNullOrWhiteSpace(userData.CookieId) ||
                string.IsNullOrWhiteSpace(userData.RouteId))
            {
                return false;
            }

            return userData.IsValidEmail();
        }
    }
}
