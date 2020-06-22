using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.Marketo
{
    public class BulkImportStatus 
    {

        public int batchId { get; set; }
        public string importId { get; set; }
        public string status { get; set; }
        public int numOfLeadsProcessed { get; set; }
        public int numOfRowsFailed { get; set; }
        public int numOfRowsWithWarning { get; set; }
        public string message { get; set; }

    }
}


