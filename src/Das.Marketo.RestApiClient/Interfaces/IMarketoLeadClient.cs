using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Models;
using Refit;

namespace Das.Marketo.RestApiClient.Interfaces
{
    [Headers("Authorization: Bearer","Content-Type: application/json")]
    public interface IMarketoLeadClient
    {
        [Get("/leads/describe.json")]
        Task<Response<LeadAttribute>> Describe();
        [Get("/lead/{id}.json")]
        Task<ResponseOfLead> Get(int id);
        [Post("/leads/push.json")]
        Task<ResponseOfPushLeadToMarketo> PushLead(PushLeadToMarketoRequest pushLead);
        [Post("/leads/push.json")]
        Task<ResponseOfPushLeadToMarketo> PushEmployerRegistrationLead(PushEmployerRegistrationLeadToMarketoRequest pushLead);
        [Post("/leads/{id}/associate.json")]
        Task<ResponseWithoutResult> AssociateLead(int id, [AliasAs("cookie")]string cookieId);
    }
}
