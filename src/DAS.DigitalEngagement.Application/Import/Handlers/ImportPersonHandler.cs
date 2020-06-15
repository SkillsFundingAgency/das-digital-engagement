using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.Marketo;
using LINQtoCSV;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportPersonHandler : IImportPersonHandler
    {
        private readonly IChunkingService _chunkingService;
        private readonly ICsvService _csvService;
        private readonly IMarketoBulkImportService _bulkImportService;
        private readonly ILogger<ImportPersonHandler> _logger;

        public ImportPersonHandler(IChunkingService chunkingService, ICsvService csvService, IMarketoBulkImportService bulkImportService, ILogger<ImportPersonHandler> logger)
        {
            _chunkingService = chunkingService;
            _csvService = csvService;
            _bulkImportService = bulkImportService;
            _logger = logger;
        }

        public async Task Handle(Stream personCsv)
        {
            _logger.LogInformation($"about to handle person import");

            var contacts = await _csvService.ConvertToList<NewLead>(personCsv);

            var contactsChunks = _chunkingService.GetChunks(personCsv.Length, contacts).ToList();

            var index = 1;

            foreach (var contactsList in contactsChunks)
            {
              var importResult = await _bulkImportService.ImportLeads(contactsList);

              _logger.LogInformation($"Bulk import chunk {index} of {contactsChunks.Count()} has been queued. \n Job details: {importResult} ");

            index++;
            }


        }

       
    }
}
