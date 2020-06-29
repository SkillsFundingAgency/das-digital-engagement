using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerUsers.Api.Client;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportEmployerUsersHandler : IImportEmployerUsersHandler
    {
        private readonly IChunkingService _chunkingService;
        private readonly IBulkImportService _bulkImportService;
        private readonly IEmployerUsersApiClient _employerUsersApiClient;
        private readonly IReportService _reportService;
        private readonly ILogger<ImportPersonHandler> _logger;

        public ImportEmployerUsersHandler(IChunkingService chunkingService, IBulkImportService bulkImportService, ILogger<ImportPersonHandler> logger, IReportService reportService, IEmployerUsersApiClient employerUsersApiClient)
        {
            _chunkingService = chunkingService;
            _bulkImportService = bulkImportService;
            _logger = logger;
            _reportService = reportService;
            _employerUsersApiClient = employerUsersApiClient;
        }

        public async Task<BulkImportFileStatus> Handle()
        {
            _logger.LogInformation($"about to handle person import");

            var fileStatus = new BulkImportFileStatus();

            var contacts = await _employerUsersApiClient.GetPageOfEmployerUsers(1, 9999999);



;            var contactsChunks = _chunkingService.GetChunks(contacts.Data.Count, contacts.Data).ToList();

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
