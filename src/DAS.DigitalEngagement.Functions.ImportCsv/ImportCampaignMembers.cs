using System;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportCampaignMembers
    {
        private readonly IImportCampaignMembersHandler _importCampaignMembersHandler;
        private readonly IReportService _reportService;
        private readonly IBlobService _blobService;
        private readonly string _container = "import-campaign-members";
        public ImportCampaignMembers(IImportCampaignMembersHandler importCampaignMembersHandler, IReportService reportService, IBlobService blobService)
        {
            _importCampaignMembersHandler = importCampaignMembersHandler;
            _reportService = reportService;
            _blobService = blobService;
        }
        [FunctionName("ImportCampaignMembers")]
        public async Task Run([BlobTrigger("import-campaign-members/{name}")]Stream myBlob, [DurableClient] IDurableOrchestrationClient starter, Binder binder, string name, ILogger log)
        {
            if (name.Contains(".report.txt") == false)
            {


                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                string report;
                try
                {
                    var importJobs = await _importCampaignMembersHandler.Handle(myBlob, name.Replace(".csv", ""));

                    importJobs.Id = Guid.NewGuid().ToString();
                    importJobs.Name = name;
                    importJobs.Container = _container;

                    report = _reportService.CreateImportReport(importJobs);
                    string instanceId = await starter.StartNewAsync("MonitorBulkImport", importJobs.Id, importJobs);

                    myBlob.Close();

                    await _blobService.DeleteFile(name, _container);
                }
                catch (Exception ex)
                {
                    report = $"Unable to import person CSV: {ex}";
                    log.LogError(report);
                    throw;
                }


                var attributes = new Attribute[]
                {
                new BlobAttribute($"{_container}/Report/{name}.report.txt", FileAccess.Write),
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
