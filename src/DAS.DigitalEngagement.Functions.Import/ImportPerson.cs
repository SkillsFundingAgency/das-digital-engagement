using System.Text;
using Azure.Storage.Blobs;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    namespace DAS.DigitalEngagement.Functions.Import
    {
        public class ImportPerson
        {
            private readonly IImportPersonHandler _importPersonHandler;
            private readonly IReportService _reportService;
            private readonly IBlobService _blobService;
            private readonly string _container = "import-person";
            private readonly BlobServiceClient _blobServiceClient;
            private readonly ILogger<ImportPerson> _logger;

            public ImportPerson(IImportPersonHandler importPersonHandler,
                IReportService reportService,
                IBlobService blobService,
                BlobServiceClient blobServiceClient,
                ILogger<ImportPerson> logger)
            {
                _importPersonHandler = importPersonHandler;
                _reportService = reportService;
                _blobService = blobService;
                _blobServiceClient = blobServiceClient;
                _logger = logger;
            }

            [Function("ImportPerson")]
            public async Task Run([BlobTrigger("import-person/{name}")] Stream myBlob,
                [DurableClient] DurableTaskClient starter, string name)
            {

                if (name.Contains(".report.txt") == false)
                {
                    _logger.LogInformation(
                        $"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                    string report;
                    try
                    {
                        var importJobs = await _importPersonHandler.Handle(myBlob);

                        importJobs.Id = Guid.NewGuid().ToString();
                        importJobs.Name = name;
                        importJobs.Container = _container;

                        report = _reportService.CreateImportReport(importJobs);

                        if (importJobs.ImportFileIsValid)
                        {
                            await starter.ScheduleNewOrchestrationInstanceAsync("MonitorBulkImport", importJobs);
                        }

                        myBlob.Close();

                        await _blobService.DeleteFile(name, _container);

                    }
                    catch (Exception ex)
                    {
                        report = $"Unable to import person CSV: {ex}";
                        _logger.LogError(report);
                        throw;
                    }

                    await SaveReportToBlob(report, name);
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
}
