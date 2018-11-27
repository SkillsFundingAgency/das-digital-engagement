using System;
using System.Threading.Tasks;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers
{
    public class RegisterHandler : IRegisterHandler
    {
        private readonly IUserDataValidator _validator;
        private readonly IUserService _userServiceObject;
        private readonly IWiredPlusService _wiredPlusService;

        public RegisterHandler(IUserDataValidator validator, IUserService userServiceObject, IWiredPlusService wiredPlusService)
        {
            _validator = validator;
            _userServiceObject = userServiceObject;
            _wiredPlusService = wiredPlusService;
        }

        public async Task Handle(UserData userData)
        {
            if(!_validator.Validate(userData))
            {
                throw new ArgumentException("UserData model failed validation", nameof(UserData));
            }

            await _userServiceObject.RegisterUser(userData);

            await _wiredPlusService.CreateUser(userData);
        }
    }
}
