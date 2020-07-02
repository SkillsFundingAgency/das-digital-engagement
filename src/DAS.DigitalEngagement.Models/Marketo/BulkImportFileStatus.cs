using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;

namespace DAS.DigitalEngagement.Models.Marketo
{
    public class BulkImportFileStatus
    {
        public BulkImportFileStatus()
        {
            StartTime = DateTime.Now;
            BulkImportJobs = new List<BulkImportJob>();
        }
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public IList<BulkImportJob> BulkImportJobs { get; set; }
        public double Duration => (DateTime.Now - StartTime).TotalMilliseconds;
        public ImportStatus Status
        {
            get
            {
                var status = ImportStatus.Queued;


                if (BulkImportJobs.Any(s => s.Status == "Failed"))
                {
                    status = ImportStatus.Failed;
                }
                else if (BulkImportJobs.Any(s => s.Status == "Importing"))
                {
                    status = ImportStatus.Processing;
                }
                else if (BulkImportJobs.All(s => s.Status == "Complete"))
                {
                    status = ImportStatus.Completed;
                }
                
                return status;
            }
        }

        
    }
}