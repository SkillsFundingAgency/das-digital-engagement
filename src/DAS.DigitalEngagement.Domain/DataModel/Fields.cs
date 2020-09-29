using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Domain.DataModel
{
    public class Fields
    {
        public string displayName { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string dataType { get; set; }
        public FieldRelationship relatedTo { get; set; }
        public bool isDedupeField { get; set; }
    }
}