using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Domain.DataModel
{
    public class Table
    {
        public string action { get; set; }
        public string displayName { get; set; }
        public string apiName { get; set; }
        public string description { get; set; }
        public IEnumerable<Fields> fields { get; set; }
    }
}
