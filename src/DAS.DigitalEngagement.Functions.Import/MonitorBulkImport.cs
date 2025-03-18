using System.Net;
using System.Text.Json;
using Azure.Storage.Blobs;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Azure.Functions.Worker.Http;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class MonitorBulkImport
    {
        private readonly IReportService _reportService;
        private readonly IBulkImportService _bulkImportService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<MonitorBulkImport> _logger;

        public MonitorBulkImport(IReportService reportService,
            IBulkImportService bulkImportService,
            BlobServiceClient blobServiceClient,
            ILogger<MonitorBulkImport> logger)
        {
            _reportService = reportService;
            _bulkImportService = bulkImportService;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        [Function("MonitorBulkImport")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var outputs = new List<BulkImportJobStatus>();
            var input = context.GetInput<BulkImportStatus>();

            int pollingInterval = 5;

            while (input.BulkImportJobs.Any(s => s.Status != "Complete"))
            {
                outputs.Clear();

                foreach (var bulkImportJob in input.BulkImportJobs.ToList())
                {
                    outputs.Add(await context.CallActivityAsync<BulkImportJobStatus>("MonitorBulkImport_Job", bulkImportJob));
                }

                input.BulkImportJobStatus = outputs;

                await context.CallActivityAsync<BulkImportJob>("MonitorBulkImport_Report", input);

                if (input.BulkImportJobStatus.All(s => s.Status == "Complete"))
                {
                    break;
                }

                var nextCheck = context.CurrentUtcDateTime.AddSeconds(pollingInterval);
                await context.CreateTimer(nextCheck, CancellationToken.None);
            }
        }

        [Function("MonitorBulkImport_Job")]
        public async Task<BulkImportJobStatus> MoitorJob([ActivityTrigger] BulkImportJob job, ILogger log)
        {

            var jobStatus = await _bulkImportService.GetJobStatus(job.batchId);

            return jobStatus;
        }

        [Function("MonitorBulkImport_HttpStart")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient starter)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<BulkImportStatus>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync("MonitorBulkImport", data);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Orchestration started with ID = '{instanceId}'.");

            return response;
        }

        [Function("MonitorBulkImport_Report")]
        public async Task Run([ActivityTrigger] BulkImportStatus myJobs)
        {
            try
            {
                var report = _reportService.CreateImportReport(myJobs);

                var containerClient = _blobServiceClient.GetBlobContainerClient(myJobs.Container);
                await containerClient.CreateIfNotExistsAsync(); 

                var reportBlobClient = containerClient.GetBlobClient($"Report/{myJobs.Name}.report.txt");

                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(report));
                await reportBlobClient.UploadAsync(stream, overwrite: true);

                _logger.LogInformation($"Report saved to blob: Report/{myJobs.Name}.report.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving report to blob storage.");
                throw;
            }
        }
    }
}

