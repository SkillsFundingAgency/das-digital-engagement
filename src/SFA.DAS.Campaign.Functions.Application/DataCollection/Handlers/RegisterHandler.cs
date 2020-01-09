﻿using System;
using System.Threading.Tasks;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Models.DataCollection;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers
{
    public class RegisterHandler : IRegisterHandler
    {
        private readonly IUserDataValidator _validator;
        private readonly IUserService _userServiceObject;
        private readonly IMarketoService _marketoService;

        public RegisterHandler(IUserDataValidator validator, IUserService userServiceObject, IMarketoService marketoService)
        {
            _validator = validator;
            _userServiceObject = userServiceObject;
            _marketoService = marketoService;
        }

        public async Task Handle(UserData userData)
        {
            if(!_validator.Validate(userData))
            {
                throw new ArgumentException("UserData model failed validation", nameof(UserData));
            }

            await _marketoService.PushLead(userData);
            await _userServiceObject.RegisterUser(userData);

           
        }
    }
}
