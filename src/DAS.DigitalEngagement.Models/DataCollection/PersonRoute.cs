using System;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.DataCollection
{
    public class PersonRoute
    {
        [JsonProperty("captured")]
        public string Captured { get; set; }
        [JsonProperty("routeIdentifier")]
        public string RouteIdentifier { get; set; }
    }
}