using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Services;
using Microsoft.Extensions.Options;
using Refit;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using DAS.DigitalEngagement.Models.Infrastructure;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Application.DataCollection.Services
{
    public class MarketoLeadService : IMarketoService
    {
        private readonly IMarketoLeadClient _marketoLeadClient;
        private readonly IOptions<MarketoConfiguration> _marketoOptions;

        public MarketoLeadService(IMarketoLeadClient marketoLeadClient, IOptions<MarketoConfiguration> marketoOptions)
        {
            _marketoLeadClient = marketoLeadClient;
            _marketoOptions = marketoOptions;
        }

        public async Task PushLead(UserData user)
        {
            var pushedLead = await _marketoLeadClient.PushLead(
                new PushLeadToMarketoRequest().MapFromUserData(user,
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
