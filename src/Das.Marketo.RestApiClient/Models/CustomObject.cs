using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Das.Marketo.RestApiClient.Models
{
    public class CustomObject
    {
        public string State { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ApiName { get; set; }
        public string IdField { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string[] DedupeFields { get; set; }
        public Relationship[] Relationships { get; set; }
        public IList<CustomObjectField> Fields { get; set; }
    }

    public class Relationship
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public Relatedto RelatedTo { get; set; }
    }

    public class Relatedto
    {
        public string Name { get; set; }
        public string Field { get; set; }
    }


}
