using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class PersonCookie
    {
        [JsonProperty("captured")]
        public string Captured { get; set; }
        [JsonProperty("cookieIdentifier")]
        public string CookieIdentifier { get; set; }
    }
}