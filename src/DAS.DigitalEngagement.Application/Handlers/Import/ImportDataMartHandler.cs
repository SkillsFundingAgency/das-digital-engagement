using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Infrastructure.Repositories;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public interface IImportDataMartHandler
    {
        Task<BulkImportStatus> Handle(DataMartSettings config);
    }

    public class ImportDataMartHandler : IImportDataMartHandler
    {
        private readonly IDataMartRepository _dataMartRepository;
        private readonly IBulkImportService _bulkImportService;
        private readonly ILogger<ImportDataMartHandler> _logger;

        public ImportDataMartHandler(IBulkImportService bulkImportService, ILogger<ImportDataMartHandler> logger, IDataMartRepository dataMartRepository)
        {
            _bulkImportService = bulkImportService;
            _logger = logger;
            _dataMartRepository = dataMartRepository;
        }

        public async Task<BulkImportStatus> Handle(DataMartSettings config)
        {
            _logger.LogInformation($"about to handle employer lead import");


            var data = _dataMartRepository.RetrieveViewData(config.ViewName);

            if (config.ObjectName == "Lead")
            {
                var status = await _bulkImportService.ImportPeople(data);

                return status;
            }
            else
            {
                var status = await _bulkImportService.ImportCustomObject(data, config.ObjectName);

                return status;
            }
         

        }

    }
}
