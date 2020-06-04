using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.Marketo;
using LINQtoCSV;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.Import
{
    public class ImportPerson
    {
        private readonly IImportPersonHandler _importPersonHandler;
        public ImportPerson(IImportPersonHandler importPersonHandler)
        {
            _importPersonHandler = importPersonHandler;
        }
        [FunctionName("ImportPerson")]
        public async Task Run([BlobTrigger("samples-workitems/{name}")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            try
            {
               await _importPersonHandler.Handle(myBlob);
            }
            catch (Exception ex)
            {
                log.LogError($"Unable to import person CSV: {ex}");
                throw;
            }
            


        }


    }
}
