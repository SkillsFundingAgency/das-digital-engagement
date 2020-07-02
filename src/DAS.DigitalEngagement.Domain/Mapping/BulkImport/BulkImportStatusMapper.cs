using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Mapping.BulkImport
{
    public interface IBulkImportStatusMapper
    {
        BulkImportStatus Map(BatchStatus batchStatus);
    }

    public class BulkImportStatusMapper : IBulkImportStatusMapper
    {
       public BulkImportStatus Map(BatchStatus batchStatus)
       {
           var status = new BulkImportStatus();

           status.Id = batchStatus.batchId;
           status.ImportId = batchStatus.importId;
           status.Status = batchStatus.status;
           status.Message = batchStatus.message;
           status.NumOfLeadsProcessed = batchStatus.numOfLeadsProcessed;
           status.NumOfRowsWithWarning = batchStatus.numOfRowsWithWarning;
           status.NumOfRowsFailed = batchStatus.numOfRowsFailed;

           return status;
       }
    }
}
