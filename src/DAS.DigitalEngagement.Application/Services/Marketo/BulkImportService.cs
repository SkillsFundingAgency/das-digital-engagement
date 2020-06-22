using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.Infrastructure;
using DAS.DigitalEngagement.Models.Marketo;
using Microsoft.Extensions.Options;
using Refit;

namespace DAS.DigitalEngagement.Application.Services.Marketo
{
    public class BulkImportService : IMarketoBulkImportService
    {
        private readonly IMarketoBulkImportClient _marketoBulkImportClient;
        private readonly ICsvService _csvService;

        public BulkImportService(IMarketoLeadClient marketoLeadClient,
            IMarketoBulkImportClient marketoBulkImportClient, ICsvService csvService)
        {
            _marketoBulkImportClient = marketoBulkImportClient;
            _csvService = csvService;
        }

        public async Task<BulkImportJob> ImportLeads(IList<NewLead> leads)
        {
            var streamBytes = _csvService.ToCsv(leads);
            using (var stream = new MemoryStream(streamBytes))
            {

                var streamPart = new StreamPart(stream,String.Empty, "text/csv");

                var bulkImportResponse = await _marketoBulkImportClient.PushLeads(streamPart);

                if (bulkImportResponse.Success == false)
                {
                    throw new Exception(
                        $"Unable to push lead to Marketo due to errors: {bulkImportResponse.ToString()}");
                }

                return bulkImportResponse.Result.FirstOrDefault();
            }
        }
    }
}
