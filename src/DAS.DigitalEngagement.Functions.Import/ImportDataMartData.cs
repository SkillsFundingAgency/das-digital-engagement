using Azure.Storage.Blobs;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportDataMartData
    {
        private readonly IReportService _reportService;
        private readonly string _container = "import-person";
        private readonly IList<DataMartSettings> _dataMartConfigs;
        private readonly IImportDataMartHandler _importDataMartHandler;
        private readonly IConfigureDataModelHandler _configureDataModelHandler;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<ImportDataMartData> _logger;

        public ImportDataMartData(IReportService reportService,
            IBlobService blobService,
            IOptions<List<DataMartSettings>> dataMartConfigs,
            IImportDataMartHandler importDataMartHandler,
            IConfigureDataModelHandler configureDataModelHandler,
            BlobServiceClient blobServiceClient,
            ILogger<ImportDataMartData> logger)
        {
            _reportService = reportService;
            _dataMartConfigs = dataMartConfigs.Value;
            _importDataMartHandler = importDataMartHandler;
            _configureDataModelHandler = configureDataModelHandler;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        [Function("ImportDataMartData")]
        public async Task Run([TimerTrigger("%DataMartImportSchedule%")] TimerInfo timerInfo, [DurableClient] DurableTaskClient starter)
        { 
            _logger.LogInformation($"Import Datamart data Timer function triggered Schedule: {DateTime.UtcNow}");

            await _configureDataModelHandler.ConfigureDataModel(_dataMartConfigs);

            string report;
            BulkImportStatus importJobsStatus;
            foreach (var dataMartConfig in _dataMartConfigs)
            {
                try
                {
                    importJobsStatus = await _importDataMartHandler.Handle(dataMartConfig);

                    importJobsStatus.Id = Guid.NewGuid().ToString();
                    importJobsStatus.Name = $"Data-Mart/{dataMartConfig.ObjectName}/Import-{DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}";
                    importJobsStatus.Container = _container;

                    report = _reportService.CreateImportReport(importJobsStatus);
                    await starter.ScheduleNewOrchestrationInstanceAsync("MonitorBulkImport", importJobsStatus);

                }
                catch (Exception ex)
                {
                    report = $"Unable to import custom object {dataMartConfig.ObjectName}: {ex}";
                    _logger.LogError(report);
                    throw;
                }

                await SaveReportToBlob(report, importJobsStatus.Name);
            }

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
