using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Mapping.BulkImport
{
    public interface IBulkImportJobMapper
    {
        BulkImportJob Map(BatchJob batchStatus);
    }

    public class BulkImportJobMapper : IBulkImportJobMapper
    {
       public BulkImportJob Map(BatchJob batchJob)
       {
           var job = new BulkImportJob();

           job.ImportId = batchJob.ImportId;
           job.batchId = batchJob.batchId;
           job.Status = batchJob.Status;

           return job;
       }
    }
}
