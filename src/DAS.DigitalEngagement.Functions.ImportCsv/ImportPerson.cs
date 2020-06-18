using System;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportPerson
    {
        private readonly IImportPersonHandler _importPersonHandler;
        private readonly IReportService _reportService;
        public ImportPerson(IImportPersonHandler importPersonHandler, IReportService reportService)
        {
            _importPersonHandler = importPersonHandler;
            _reportService = reportService;
        }
        [FunctionName("ImportPerson")]
        public async Task Run([BlobTrigger("import-person/{name}")]Stream myBlob, [DurableClient] IDurableOrchestrationClient starter, Binder binder, string name, ILogger log)
        {
            if (name.Contains(".report.txt") == false)
            {


                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                string report;
                try
                {
                    var importJobs = await _importPersonHandler.Handle(myBlob);

                    importJobs.Id = name;

                    report = _reportService.CreateImportReport(importJobs);
                    string instanceId = await starter.StartNewAsync("MonitorBulkImport", importJobs.Id, importJobs);
                }
                catch (Exception ex)
                {
                    report = $"Unable to import person CSV: {ex}";
                    log.LogError(report);
                    throw;
                }


                var attributes = new Attribute[]
                {
                new BlobAttribute($"import-person/{name}.report.txt", FileAccess.Write),
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
