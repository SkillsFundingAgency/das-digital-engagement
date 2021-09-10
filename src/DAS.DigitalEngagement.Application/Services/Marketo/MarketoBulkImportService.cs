﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Mapping.BulkImport;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using DAS.DigitalEngagement.Models.Validation;
using Das.Marketo.RestApiClient.Interfaces;
using Microsoft.Extensions.Logging;
using Refit;
using Polly;
using Das.Marketo.RestApiClient.Models;
using Polly.Retry;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Application.Services.Marketo
{
    public class MarketoBulkImportService : IBulkImportService
    {
        private readonly IChunkingService _chunkingService;
        private readonly IMarketoBulkImportClient _marketoBulkImportClient;
        private readonly IMarketoLeadClient _marketoLeadClient;
        private readonly ICsvService _csvService;
        private readonly ILogger<MarketoBulkImportService> _logger;
        private readonly IBulkImportStatusMapper _bulkImportStatusMapper;
        private readonly IBulkImportJobMapper _bulkImportJobMapper;
        private readonly IOptions<MarketoConfiguration> _marketoOptions;

        public MarketoBulkImportService(IMarketoLeadClient marketoLeadClient,
            IMarketoBulkImportClient marketoBulkImportClient, ICsvService csvService, ILogger<MarketoBulkImportService> logger, IBulkImportStatusMapper bulkImportStatusMapper, IBulkImportJobMapper bulkImportJobMapper, IChunkingService chunkingService, IOptions<MarketoConfiguration> marketoOptions)
        {
            _marketoLeadClient = marketoLeadClient;
            _marketoBulkImportClient = marketoBulkImportClient;
            _csvService = csvService;
            _logger = logger;
            _bulkImportStatusMapper = bulkImportStatusMapper;
            _bulkImportJobMapper = bulkImportJobMapper;
            _chunkingService = chunkingService;
            _marketoOptions = marketoOptions;
        }


        public async Task<FieldValidationResult> ValidateFields(IList<string> fields)
        {
            var leadFields = await _marketoLeadClient.Describe();
            fields = fields.Select(s => s.ToLower()).ToList();

            var marketotFields = leadFields.Result.Where(w => w.Rest.ReadOnly == false).Select(s => s.Rest.Name.ToLower()).ToList();
            marketotFields.Concat(leadFields.Result.Where(w => w.Soap.ReadOnly == false).Select(s => s.Soap.Name.ToLower()));
                
            var fieldvalidationresult = new FieldValidationResult()
            {
                Errors = fields.Except(marketotFields).ToList()
            };

            return fieldvalidationresult;
        }

        public async Task<BulkImportStatus> ImportPeople<T>(IList<T> leads)
        {
            var fileStatus = new BulkImportStatus();

            var contactsChunks = _chunkingService.GetChunks(_csvService.GetByteCount(leads), leads).ToList();
            _logger.LogInformation($"ImportPeople: total of {leads.Count} leads to import, in {contactsChunks.Count} chunks");

            var index = 1;

            foreach (var contactsList in contactsChunks)
            {
                var importResult =
                    await ImportChunkedPeople(contactsList);
                fileStatus.BulkImportJobs.Add(importResult);

                _logger.LogInformation($"ImportPeople chunk {index} of {contactsChunks.Count()} has been queued. \n Job details: {importResult} ");

                index++;
            }

            return fileStatus;
        }

        public async Task<BulkImportStatus> ImportCustomObject<T>(IList<T> data, string objectName)
        {
            var fileStatus = new BulkImportStatus();

            var contactsChunks = _chunkingService.GetChunks(_csvService.GetByteCount(data), data).ToList();
            _logger.LogInformation($"ImportCustomObject: total of {data.Count} {objectName} to import, in {contactsChunks.Count} chunks");

            var runningJobsCount = 0;
            var index = 1;
            List<int> completedJobs = new List<int>();

            foreach (var contactsList in contactsChunks)
            {
                var importResult =
                    await ImportChunkedObject(contactsList, objectName);
                fileStatus.BulkImportJobs.Add(importResult);

                _logger.LogInformation($"ImportCustomObject chunk {index} of {contactsChunks.Count()} for object: {objectName} has been queued. \n Job details: {importResult} ");

                runningJobsCount++;
                index++;

                if (runningJobsCount > 9)
                {
                    bool getJobStatus = true;
                    var notCompletedJobs = fileStatus.BulkImportJobs.Where(x => !completedJobs.Any(y => y== x.batchId))
                                                                        .Select(x => x.batchId).ToList();
                    while (getJobStatus)
                    {
                        foreach (var job in notCompletedJobs)
                        {
                            var val = await _marketoBulkImportClient.GetStatus(job);
                            var status = val.Result.FirstOrDefault().status;
                            _logger.LogInformation($"BatchStatus batchId : {job} - status :{status}");
                          
                            if (status == "Complete" || status == "Failed")
                            {
                                getJobStatus = false;
                                runningJobsCount--;
                                completedJobs.Add(job);
                                break;
                            }
                            else
                            {
                                await Task.Delay(1000);
                            }
                        }
                    }
                }
            }

            return fileStatus;
        }


        private async Task<BulkImportJob> ImportChunkedPeople<T>(IList<T> leads)
        {
            var csvStrings = _csvService.ToCsv(leads);

            var bulkImportResponse = await MarketoApiJobRetryPolicy
                .ExecuteAsync(async (context) =>
                {
                    using (var stream = GenerateStreamFromString(csvStrings))
                    {
                        var streamPart = new StreamPart(stream, String.Empty, "text/csv");
                        var val = await _marketoBulkImportClient.PushLeads(streamPart);
                        return val;
                    }
                }
                , new Context("ImportChunkedPeople")
            ) ;

            if (bulkImportResponse.Success == false)
            {
                throw new Exception(
                    $"Unable to push person due to errors: {bulkImportResponse.ToString()}");
            }

            return bulkImportResponse.Result.Select(_bulkImportJobMapper.Map).FirstOrDefault();
        }

        private async Task<BulkImportJob> ImportChunkedObject<T>(IList<T> leads,string objectName)
        {
            var csvStrings = _csvService.ToCsv(leads);

            var bulkImportResponse = await MarketoApiJobRetryPolicy
                .ExecuteAsync(async (context) => {
                    using (var stream = GenerateStreamFromString(csvStrings))
                    {
                        var streamPart = new StreamPart(stream, String.Empty, "text/csv");
                        var val = await _marketoBulkImportClient.PushCustomObject(streamPart, objectName);
                        return val;
                    }
                }
                , new Context("ImportChunkedObject")
            );

            if (bulkImportResponse.Success == false)
            {
                throw new Exception(
                    $"Unable to push {objectName} due to errors: {bulkImportResponse.ToString()}");
            }

            return bulkImportResponse.Result.Select(_bulkImportJobMapper.Map).FirstOrDefault();
        }

        public async Task<BulkImportJob> ImportToCampaign<T>(IList<T> leads, string campaignId)
        {
            var csvStrings = _csvService.ToCsv(leads);

            using (var stream = GenerateStreamFromString(csvStrings))
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

        public async Task<BulkImportJobStatus> GetJobStatus(int jobId)
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

        private AsyncRetryPolicy<Response<BatchJob>> MarketoApiJobRetryPolicy
        {
            get
            {
                var retryPolicy = Policy<Response<BatchJob>>
                    .HandleResult(j => j.Success == false)
                    .WaitAndRetryAsync(_marketoOptions.Value.ApiRetryCount,
                        attempt => TimeSpan.FromSeconds(_marketoOptions.Value.ApiRetryInitialBackOffSecs * Math.Pow(2, attempt)), // Exponential Back off.
                        (response, nextRetryTimeSpan, retryCount, context) => // Capture some info for logging on each failure, before next retry
                        {
                            _logger.LogWarning($"  Marketo API call on retry: {retryCount} failed: {response.Result.ToString()}.  Retrying again in: {nextRetryTimeSpan.TotalMilliseconds}ms. Context: {context.OperationKey}");
                        });

                return retryPolicy;
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
