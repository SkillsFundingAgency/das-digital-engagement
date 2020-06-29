﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping.Marketo;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportPersonHandler : IImportPersonHandler
    {
        private readonly IChunkingService _chunkingService;
        private readonly ICsvService _csvService;
        private readonly IBulkImportService _bulkImportService;
        private readonly IReportService _reportService;
        private readonly ILogger<ImportPersonHandler> _logger;
        private readonly IPersonMapper _personMapper;

        public ImportPersonHandler(IChunkingService chunkingService, ICsvService csvService, IBulkImportService bulkImportService, ILogger<ImportPersonHandler> logger, IReportService reportService)
        {
            _chunkingService = chunkingService;
            _csvService = csvService;
            _bulkImportService = bulkImportService;
            _logger = logger;
            _reportService = reportService;
        }

        public async Task<BulkImportFileStatus> Handle(Stream personCsv)
        {
            _logger.LogInformation($"about to handle person import");

            var fileStatus = new BulkImportFileStatus();
            var contacts = await _csvService.ConvertToList<Person>(personCsv);
            var contactsChunks = _chunkingService.GetChunks(personCsv.Length, contacts).ToList();

            var index = 1;

            foreach (var contactsList in contactsChunks)
            {
                var importResult = await _bulkImportService.ImportPeople(contactsList);
                fileStatus.BulkImportJobs.Add(importResult);

                _logger.LogInformation($"Bulk import chunk {index} of {contactsChunks.Count()} has been queued. \n Job details: {importResult} ");

                index++;
            }

            return fileStatus;
        }


    }
}
