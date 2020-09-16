using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Das.Marketo.RestApiClient.Models.Requests
{
    public class CreateCustomObjectFieldsRequest
    {
        [JsonProperty("input")]
        public IList<CustomObjectFields> Input { get; set; }
    }


    public class CustomObjectFields
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dataType")]
        public string DataType { get; set; }
        [JsonProperty("relatedTo")]
        public Relatedto RelatedTo { get; set; }
        [JsonProperty("isDedupeField")]
        public bool IsDedupeField { get; set; }
    }

    public class Relatedto
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
