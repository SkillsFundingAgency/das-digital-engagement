using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using SFA.DAS.Campaign.Functions.Models.Marketo;

namespace SFA.DAS.Campaign.Functions.Application.Services
{
    [Headers("Authorization: Bearer","Content-Type: application/json")]
    public interface IMarketoLeadClient
    {
        [Get("/lead/{id}.json")]
        Task<ResponseOfLead> Get(int id);
        [Post("/leads/push.json")]
        Task<ResponseOfPushLeadToMarketo> PushLead(PushLeadToMarketoRequest pushLead);
        [Post("/leads/{id}/associate.json")]
        Task<ResponseWithoutResult> AssociateLead(int id, [AliasAs("cookie")]string cookieId);
    }
}
