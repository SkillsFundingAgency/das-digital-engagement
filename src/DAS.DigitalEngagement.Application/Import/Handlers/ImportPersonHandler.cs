using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

            using (var sr = new StreamReader(personCsv))
            {
                fileStatus = await ValidateImportStream(sr);

                if (fileStatus.ImportFileIsValid == false)
                {
                    return fileStatus;
                }

                IList<dynamic> contacts = null;
                try
                {
                    contacts = await _csvService.ConvertToList(sr);


                }
                catch (Exception e)
                {
                    _logger.LogError(e,"Unable to process csv file");

                    var status = new BulkImportStatus();
                    status.ImportFileIsValid = false;
                    status.ValidationError = "Unable to parse CSV file, the format of the file is invalid";
                    return status;
                }

                return await _bulkImportService.ImportPeople(contacts);
            }
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

            if (status.ValidationError != null)
            {
                status.ImportFileIsValid = false;
                return status;
            }
         

            IList<string> csvFields = CsvFields(sr);
            
            var fieldValidation = await _bulkImportService.ValidateFields(csvFields);

            if (fieldValidation.IsValid == false)
            {
               status.HeaderErrors = fieldValidation.Errors;
               
            }

            if (status.HeaderErrors.Any())
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
