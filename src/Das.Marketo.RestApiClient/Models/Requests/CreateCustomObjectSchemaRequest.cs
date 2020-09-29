using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Das.Marketo.RestApiClient.Models.Requests
{
    public class CreateCustomObjectSchemaRequest
    {
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("apiName")]
        public string ApiName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }


}
