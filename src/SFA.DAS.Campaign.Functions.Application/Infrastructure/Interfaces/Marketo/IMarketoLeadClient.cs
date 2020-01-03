using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using SFA.DAS.Campaign.Functions.Models.Marketo;

namespace SFA.DAS.Campaign.Functions.Application.Services
{
    public interface IMarketoLeadClient
    {
        [Get("/rest/v1/lead/{id}.json")]
        ResponseOfLead Get(string id);
        [Post("/rest/v1/leads/push.json")]
        ResponseOfPushLeadToMarketo PushLead(PushLeadToMarketoRequest pushLead);
        [Post("/rest/v1/leads/{id}/associate.json?cookie={cookieId}")]
        ResponseWithoutResult AssociateLead(string id, string cookieId);
    }
}
