using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Azure.Storage.Blobs;
using System.Text;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportEmployerUsers
    {
        private readonly IImportEmployerUsersHandler _importEmployerUsersHandler;
        private readonly IReportService _reportService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<ImportEmployerUsers> _logger;
        private readonly string _container = "import-person";

        public ImportEmployerUsers(IReportService reportService,
            IBlobService blobService,
            IImportEmployerUsersHandler importEmployerUsersHandler,
            BlobServiceClient blobServiceClient,
            ILogger<ImportEmployerUsers> logger)
        {
            _reportService = reportService;
            _importEmployerUsersHandler = importEmployerUsersHandler;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task Run([TimerTrigger("%EmployerUsersImportSchedule%")] TimerInfo myTimer, [DurableClient] DurableTaskClient starter, ILogger log)
        {

            log.LogInformation($"Import Employer Users Timer function triggered, Schedule: {DateTime.UtcNow}");

            string report;
            BulkImportStatus importJobsStatus;
            try
            {
                importJobsStatus = await _importEmployerUsersHandler.Handle();

                importJobsStatus.Id = Guid.NewGuid().ToString();
                importJobsStatus.Name = $"Employer-Users/Import-{DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}";
                importJobsStatus.Container = _container;

                report = _reportService.CreateImportReport(importJobsStatus);
                await starter.ScheduleNewOrchestrationInstanceAsync("MonitorBulkImport", importJobsStatus);
            }
            catch (Exception ex)
            {
                report = $"Unable to import Employer Users: {ex}";
                log.LogError(report);
                throw;
            }

            await SaveReportToBlob(report, importJobsStatus.Name);
        }
        private async Task SaveReportToBlob(string reportContent, string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_container);
                await containerClient.CreateIfNotExistsAsync();

                var reportBlobClient = containerClient.GetBlobClient($"Report/{fileName}.report.txt");

                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));
                await reportBlobClient.UploadAsync(stream, overwrite: true);

                _logger.LogInformation($"Report file saved: {fileName}.report.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save report file.");
                throw;
            }
        }
    }
}
