using System;
using System.Threading.Tasks;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers
{
    public class UnregisterHandler : IUnregisterHandler
    {
        private readonly IUserUnregisterDataValidator _validator;
        private readonly IUserService _userService;
        private readonly IWiredPlusService _wiredPlusService;

        public UnregisterHandler(IUserUnregisterDataValidator validator, IUserService userService, IWiredPlusService wiredPlusService)
        {
            _validator = validator;
            _userService = userService;
            _wiredPlusService = wiredPlusService;
        }

        public async Task Handle(UserData userData)
        {
            if (!_validator.Validate(userData.Email))
            {
                throw new ArgumentException("UserData model failed validation", nameof(UserData.Email));
            }

            await _userService.UpdateUser(userData);

            if (userData.Consent)
            {
                await _wiredPlusService.SubscribeUser(userData);
            }
            else
            {
                await _wiredPlusService.UnsubscribeUser(userData);
            }
            
        }
    }
}