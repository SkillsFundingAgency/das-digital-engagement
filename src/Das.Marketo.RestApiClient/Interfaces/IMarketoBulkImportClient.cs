using System.Net.Http;
using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Models;
using Refit;

namespace Das.Marketo.RestApiClient.Interfaces
{
    [Headers("Authorization: Bearer","Content-Type: application/json")]
    public interface IMarketoBulkImportClient
    {
        [Get("/leads/batch/{id}.json")]
        Task<Response<BatchStatus>> GetStatus([AliasAs("id")] int batchId);

        [Multipart("-------Boundary")]
        [Headers("Content-Type: multipart/form-data; boundary=-------Boundary")]
        [Post("/leads.json?format=csv")]
        Task<Response<BatchJob>> PushLeads([AliasAs("file")]StreamPart pushLeadsPart);

        [Multipart("-------Boundary")]
        [Headers("Content-Type: multipart/form-data; boundary=-------Boundary")]
        [Post("/program/{programId}/members/import.json?format=csv&programMemberStatus=Member")]
        Task<Response<BatchJob>> PushToProgram([AliasAs("file")]StreamPart pushLeadsPart, [AliasAs("programId")] string campaignId);

        [Get("/leads/batch/{id}/warnings.json")]
        Task<HttpContent> GetWarnings([AliasAs("id")] int batchId);

        [Get("/leads/batch/{id}/failures.json")]
        Task<HttpContent> GetFailures([AliasAs("id")] int batchId);

    }
}
