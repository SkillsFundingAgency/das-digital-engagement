using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Services;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;
using Refit;

namespace DAS.DigitalEngagement.Application.Services.Marketo
{
    public class BulkImportService : IBulkImportService
    {
        private readonly IMarketoBulkImportClient _marketoBulkImportClient;
        private readonly ICsvService _csvService;

        public BulkImportService(IMarketoLeadClient marketoLeadClient,
            IMarketoBulkImportClient marketoBulkImportClient, ICsvService csvService)
        {
            _marketoBulkImportClient = marketoBulkImportClient;
            _csvService = csvService;
        }

        public async Task<BulkImportJob> ImportPeople(IList<NewLead> leads)
        {
            var streamBytes = _csvService.ToCsv(leads);
            using (var stream = new MemoryStream(streamBytes))
            {

                var streamPart = new StreamPart(stream,String.Empty, "text/csv");

                var bulkImportResponse = await _marketoBulkImportClient.PushLeads(streamPart);

                if (bulkImportResponse.Success == false)
                {
                    throw new Exception(
                        $"Unable to push person due to errors: {bulkImportResponse.ToString()}");
                }

                return bulkImportResponse.Result.FirstOrDefault();
            }
        }

        public Task<BulkImportStatus> GetJobStatus(int jobId)
        {
            throw new NotImplementedException();
        }
    }
}
