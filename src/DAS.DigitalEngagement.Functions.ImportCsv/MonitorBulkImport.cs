using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Services;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class MonitorBulkImport
    {
        private readonly IReportService _reportService;

        public MonitorBulkImport(IReportService reportService)
        {
            _reportService = reportService;
        }

        [FunctionName("MonitorBulkImport")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<BulkImportJob>();
            var input = context.GetInput<BulkImportFileStatus>();

            int pollingInterval = 5;


            while (input.BulkImportJobs.Any(s => s.Status != "Complete"))
            {
                
                foreach (var bulkImportJob in input.BulkImportJobs.ToList())
                {
                    outputs.Add(await context.CallActivityAsync<BulkImportJob>("MonitorBulkImport_Job", bulkImportJob));
                }

                input.BulkImportJobs = outputs;

                await context.CallActivityAsync<BulkImportJob>("MonitorBulkImport_Report", input);


                if (input.BulkImportJobs.All(s => s.Status == "Complete"))
                {
                    break;
                }

                // Orchestration sleeps until this time.
                var nextCheck = context.CurrentUtcDateTime.AddSeconds(pollingInterval);
                await context.CreateTimer(nextCheck, CancellationToken.None);
            }
        }


        [FunctionName("MonitorBulkImport_Job")]
        public BulkImportJob MoitorJob([ActivityTrigger] BulkImportJob job, ILogger log)
        {
            return job;
        }

        [FunctionName("MonitorBulkImport_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var data = await req.Content.ReadAsAsync<IList<BulkImportJob>>();

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("MonitorBulkImport", data);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter?.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("MonitorBulkImport_Report")]
        public async Task Run([ActivityTrigger]BulkImportFileStatus myJobs, Binder binder, ILogger log)
        {


            var report = _reportService.CreateImportReport(myJobs);

            var attributes = new Attribute[]
            {
                new BlobAttribute($"import-person/{myJobs.Id}.report.txt", FileAccess.Write),
                new StorageAccountAttribute("Storage")
            };
            using (var writer = await binder.BindAsync<TextWriter>(attributes))
            {
                await writer.WriteAsync(report);
            }

        }
    }
}