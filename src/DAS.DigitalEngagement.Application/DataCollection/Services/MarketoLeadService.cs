using System;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.DataCollection.Mapping;
using Microsoft.Extensions.Options;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using Das.Marketo.RestApiClient.Configuration;
using Das.Marketo.RestApiClient.Interfaces;

namespace DAS.DigitalEngagement.Application.DataCollection.Services
{
    public class MarketoLeadService : IMarketoService
    {
        private readonly IMarketoLeadClient _marketoLeadClient;
        private readonly IOptions<MarketoConfiguration> _marketoOptions;
        private readonly UserDataMapping _userDataMapping;

        public MarketoLeadService(IMarketoLeadClient marketoLeadClient, IOptions<MarketoConfiguration> marketoOptions)
        {
            _marketoLeadClient = marketoLeadClient;
            _marketoOptions = marketoOptions;
            _userDataMapping = new UserDataMapping();
        }

        public async Task PushLead(UserData user)
        {
            var pushedLead = await _marketoLeadClient.PushLead(_userDataMapping.MapFromUserData(user,
                    _marketoOptions.Value.RegisterInterestProgramConfiguration));

            if (pushedLead.Success == false)
            {
                throw new Exception($"Unable to push lead to Marketo due to errors: {pushedLead.Errors}");
            }

            if (String.IsNullOrWhiteSpace(user.MarketoCookieId) == false)
            {
                var leadAssociated =
                    await _marketoLeadClient.AssociateLead(pushedLead.Result.First().Id, user.MarketoCookieId);

                if (leadAssociated.Success == false)
                {
                    throw new Exception($"Unable to associate lead to Marketo due to errors: {leadAssociated.Errors}");
                }
            }
        }
    }
}
