using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Mapping.BulkImport;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;
using Refit;

namespace DAS.DigitalEngagement.Application.Services
{
    public class BulkImportService : IBulkImportService
    {
        private readonly IMarketoBulkImportClient _marketoBulkImportClient;
        private readonly ICsvService _csvService;
        private readonly ILogger<BulkImportService> _logger;
        private readonly IBulkImportStatusMapper _bulkImportStatusMapper;
        private readonly IBulkImportJobMapper _bulkImportJobMapper;

        public BulkImportService(IMarketoLeadClient marketoLeadClient,
            IMarketoBulkImportClient marketoBulkImportClient, ICsvService csvService, ILogger<BulkImportService> logger, IBulkImportStatusMapper bulkImportStatusMapper, IBulkImportJobMapper bulkImportJobMapper)
        {
            _marketoBulkImportClient = marketoBulkImportClient;
            _csvService = csvService;
            _logger = logger;
            _bulkImportStatusMapper = bulkImportStatusMapper;
            _bulkImportJobMapper = bulkImportJobMapper;
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




                return bulkImportResponse.Result.Select(_bulkImportJobMapper.Map).FirstOrDefault();
            }
        }

        public async Task<BulkImportJob> ImportToCampaign(IList<NewLead> leads, string campaignId)
        {
            

            var streamBytes = _csvService.ToCsv(leads);
            using (var stream = new MemoryStream(streamBytes))
            {

                var streamPart = new StreamPart(stream, String.Empty, "text/csv");

                var bulkImportResponse = await _marketoBulkImportClient.PushToProgram(streamPart,campaignId);

                if (bulkImportResponse.Success == false)
                {
                    throw new Exception(
                        $"Unable to push person to campaign {campaignId} due to errors: {bulkImportResponse.ToString()}");
                }




                return bulkImportResponse.Result.Select(_bulkImportJobMapper.Map).FirstOrDefault();
            }
        }

        public async Task<BulkImportStatus> GetJobStatus(int jobId)
        {
            var response = await _marketoBulkImportClient.GetStatus(jobId);

            if (response.Success == false)
            {
                _logger.LogError($"Error calling API to get bulk import job status. Details: {response.ToString()}");
            }

            var status = response.Result.Select(_bulkImportStatusMapper.Map).FirstOrDefault();

            if (status.NumOfRowsFailed > 0)
            {
                status.Failures = await GetFailures(status.Id);
            }

            if (status.NumOfRowsWithWarning > 0)
            {
                status.Warnings = await GetWarnings(status.Id);
            }

            return status;
        }

        public async Task<string> GetWarnings(int jobId)
        {
            var warningsResponse = await _marketoBulkImportClient.GetWarnings(jobId);

            return await warningsResponse.ReadAsStringAsync();
        }

        public async Task<string> GetFailures(int jobId)
        {
            var failureResponse = await _marketoBulkImportClient.GetFailures(jobId);

            return await failureResponse.ReadAsStringAsync();
        }
    }
}
