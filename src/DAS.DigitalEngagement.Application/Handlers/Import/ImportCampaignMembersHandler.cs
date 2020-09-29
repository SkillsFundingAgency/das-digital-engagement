using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportCampaignMembersHandler : IImportCampaignMembersHandler
    {
        private readonly IChunkingService _chunkingService;
        private readonly ICsvService _csvService;
        private readonly IBulkImportService _bulkImportService;
        private readonly ILogger<ImportCampaignMembersHandler> _logger;

        public ImportCampaignMembersHandler(IChunkingService chunkingService, ICsvService csvService, IBulkImportService bulkImportService, ILogger<ImportCampaignMembersHandler> logger)
        {
            _chunkingService = chunkingService;
            _csvService = csvService;
            _bulkImportService = bulkImportService;
            _logger = logger;
        }

        public async Task<BulkImportStatus> Handle(Stream personCsv, string campaignId)
        {
            _logger.LogInformation($"about to handle campaign members import");

            var fileStatus = new BulkImportStatus();


            using (var sr = new StreamReader(personCsv))
            {
                fileStatus = await ValidateImportStream(sr);

                if (fileStatus.ImportFileIsValid == false)
                {
                    return fileStatus;
                }



                var contacts = await _csvService.ConvertToList(sr);
                var contactsChunks = _chunkingService.GetChunks(sr.BaseStream.Length, contacts).ToList();

                var index = 1;

                foreach (var contactsList in contactsChunks)
                {
                    var importResult = await _bulkImportService.ImportToCampaign(contactsList, campaignId);
                    fileStatus.BulkImportJobs.Add(importResult);

                    _logger.LogInformation($"Bulk import chunk {index} of {contactsChunks.Count()} to campaign ID {campaignId} has been queued. \n Job details: {importResult} ");

                    index++;
                }
            }
            return fileStatus;
        }

        private async Task<BulkImportStatus> ValidateImportStream(StreamReader sr)
        {

            var status = new BulkImportStatus();

            if (_csvService.IsEmpty(sr))
            {
                status.ValidationError = "No headers - File is empty so cannot be processed";
            }

            if (_csvService.HasData(sr))
            {
                status.ValidationError = "Missing data - there is no data to process";

            }

            IList<string> csvFields = CsvFields(sr);

            var fieldValidation = await _bulkImportService.ValidateFields(csvFields);

            if (fieldValidation.IsValid == false)
            {
                status.HeaderErrors = fieldValidation.Errors;

            }

            if (status.ValidationError != null || status.HeaderErrors.Any())
            {
                status.ImportFileIsValid = false;
            }
            return status;
        }

        private static List<string> CsvFields(StreamReader sr)
        {
            sr.BaseStream.Position = 0;
            sr.DiscardBufferedData();

            return sr.ReadLine().Split(',').ToList();
        }
    }
}
