using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Application.Services
{
    [Headers("Authorization: Bearer","Content-Type: application/json")]
    public interface IMarketoBulkImportClient
    {
        [Get("/leads/batch/{id}.json")]
        Task<Response<BulkImportStatus>> GetStatus([AliasAs("id")] int batchId);
        [Multipart("-------Boundary")]
        [Headers("Content-Type: multipart/form-data; boundary=-------Boundary")]
        [Post("/leads.json?format=csv")]
        Task<Response<BulkImportJob>> PushLeads([AliasAs("file")]StreamPart pushLeadsPart);
    }
}
