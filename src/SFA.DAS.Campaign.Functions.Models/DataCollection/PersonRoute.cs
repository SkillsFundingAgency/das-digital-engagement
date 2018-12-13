using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class PersonRoute
    {
        [JsonProperty("captured")]
        public string Captured { get; set; }
        [JsonProperty("routeIdentifier")]
        public string RouteIdentifier { get; set; }
    }
}