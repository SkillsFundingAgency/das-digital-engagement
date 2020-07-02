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
        private readonly IReportService _reportService;
        private readonly ILogger<ImportCampaignMembersHandler> _logger;

        public ImportCampaignMembersHandler(IChunkingService chunkingService, ICsvService csvService, IBulkImportService bulkImportService, ILogger<ImportCampaignMembersHandler> logger, IReportService reportService)
        {
            _chunkingService = chunkingService;
            _csvService = csvService;
            _bulkImportService = bulkImportService;
            _logger = logger;
            _reportService = reportService;
        }

        public async Task<BulkImportFileStatus> Handle(Stream personCsv, string campaignId)
        {
            _logger.LogInformation($"about to handle campaign members import");

            var fileStatus = new BulkImportFileStatus();
            var contacts = await _csvService.ConvertToList<NewLead>(personCsv);
            var contactsChunks = _chunkingService.GetChunks(personCsv.Length, contacts).ToList();

            var index = 1;

            foreach (var contactsList in contactsChunks)
            {
                var importResult = await _bulkImportService.ImportToCampaign(contactsList, campaignId);
                fileStatus.BulkImportJobs.Add(importResult);

                _logger.LogInformation($"Bulk import chunk {index} of {contactsChunks.Count()} to campaign ID {campaignId} has been queued. \n Job details: {importResult} ");

                index++;
            }

            return fileStatus;
        }


    }
}
