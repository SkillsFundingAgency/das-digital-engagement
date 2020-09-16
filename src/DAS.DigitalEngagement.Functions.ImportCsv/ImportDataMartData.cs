using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportDataMartData
    {
        private readonly IReportService _reportService;
        private readonly string _container = "import-person";
        private readonly IList<DataMartSettings> _dataMartConfigs;
        private readonly IImportDataMartHandler _importDataMartHandler;
        private readonly IConfigureDataModelHandler _configureDataModelHandler;

        public ImportDataMartData(IReportService reportService, IBlobService blobService, IOptions<List<DataMartSettings>> dataMartConfigs, IImportDataMartHandler importDataMartHandler, IConfigureDataModelHandler configureDataModelHandler)
        {
            _reportService = reportService;
            _dataMartConfigs = dataMartConfigs.Value;
            _importDataMartHandler = importDataMartHandler;
            _configureDataModelHandler = configureDataModelHandler;
        }

        [FunctionName("ImportDataMartData")]
        public async Task Run([TimerTrigger("%Functions:DataMartImportSchedule%")]TimerInfo myTimer, [DurableClient] IDurableOrchestrationClient starter, Binder binder, ILogger log)
        {

            log.LogInformation($"Import Datamart data Timer function triggered, Schedule: {myTimer.Schedule.ToString()}");

            await _configureDataModelHandler.ConfigureDataModel(_dataMartConfigs);

            string report;
            BulkImportStatus importJobsStatus;
            foreach (var dataMartConfig in _dataMartConfigs)
            {
                try
                {
                    importJobsStatus = await _importDataMartHandler.Handle(dataMartConfig);

                    importJobsStatus.Id = Guid.NewGuid().ToString();
                    importJobsStatus.Name = $"Employer-Users/Import-{DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}";
                    importJobsStatus.Container = _container;


                    report = _reportService.CreateImportReport(importJobsStatus);
                    string instanceId = await starter.StartNewAsync("MonitorBulkImport", importJobsStatus.Id, importJobsStatus);

                }
                catch (Exception ex)
                {
                    report = $"Unable to import custom object {dataMartConfig.ObjectName}: {ex}";
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
}
