using System;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Application.DataCollection.Handlers
{
    public class RegisterHandler : IRegisterHandler
    {
        private readonly IUserDataValidator _validator;
        private readonly IMarketoService _marketoService;

        public RegisterHandler(IUserDataValidator validator, IMarketoService marketoService)
        {
            _validator = validator;
            _marketoService = marketoService;
        }

        public async Task Handle(UserData userData)
        {
            if(!_validator.Validate(userData))
            {
                throw new ArgumentException("UserData model failed validation", nameof(UserData));
            }

            await _marketoService.PushLead(userData);

           
        }
    }
}
