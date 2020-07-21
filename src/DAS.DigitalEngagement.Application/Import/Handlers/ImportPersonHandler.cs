using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportPersonHandler : IImportPersonHandler
    {
        private readonly ICsvService _csvService;
        private readonly IBulkImportService _bulkImportService;
        private readonly ILogger<ImportPersonHandler> _logger;
        private readonly IPersonMapper _personMapper;

        public ImportPersonHandler(ICsvService csvService, IBulkImportService bulkImportService, ILogger<ImportPersonHandler> logger)
        {
            _csvService = csvService;
            _bulkImportService = bulkImportService;
            _logger = logger;
        }

        public async Task<BulkImportStatus> Handle(Stream personCsv)
        {
            _logger.LogInformation($"about to handle person import");

            var fileStatus = new BulkImportStatus();
            var contacts = await _csvService.ConvertToList(personCsv);

            return await _bulkImportService.ImportPeople(contacts);
        }


    }
}
