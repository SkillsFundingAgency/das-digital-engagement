using System;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportEmployerUsers
    {
        private readonly IImportEmployerUsersHandler _importEmployerUsersHandler;
        private readonly IReportService _reportService;
        private readonly string _container = "import-person";

        public ImportEmployerUsers(IReportService reportService, IBlobService blobService, IImportEmployerUsersHandler importEmployerUsersHandler)
        {
            _reportService = reportService;
            _importEmployerUsersHandler = importEmployerUsersHandler;
        }
        [FunctionName("ImportEmployerUsers")]
        public async Task Run([TimerTrigger("%Functions:EmployerUsersImportSchedule%")]TimerInfo myTimer, [DurableClient] IDurableOrchestrationClient starter, Binder binder, ILogger log)
        {

            log.LogInformation($"Import Employer Users Timer function triggered, Schedule: {myTimer.Schedule.ToString()}");

            string report;
            BulkImportStatus importJobsStatus;
            try
            {
                importJobsStatus = await _importEmployerUsersHandler.Handle();

                importJobsStatus.Id = Guid.NewGuid().ToString();
                importJobsStatus.Name =  $"Employer-Users/Import-{DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}";
                importJobsStatus.Container = _container;

                report = _reportService.CreateImportReport(importJobsStatus);
                string instanceId = await starter.StartNewAsync("MonitorBulkImport", importJobsStatus.Id, importJobsStatus);


            }
            catch (Exception ex)
            {
                report = $"Unable to import Employer Users: {ex}";
                log.LogError(report);
                throw;
            }


            var attributes = new Attribute[]
            {
                new BlobAttribute($"{_container}/Report/{importJobsStatus.Name}.report.txt", FileAccess.Write),
                new StorageAccountAttribute("Storage")
            };
            using (var writer = await binder.BindAsync<TextWriter>(attributes))
            {
                await writer.WriteAsync(report);
            }

        }


    }
}
