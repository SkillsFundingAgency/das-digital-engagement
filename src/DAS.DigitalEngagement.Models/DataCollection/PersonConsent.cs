using System;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.DataCollection
{
    public class PersonConsent
    {
        [JsonProperty("gdprConsentDeclared")]
        public string GdprConsentDeclared { get; set; }
        [JsonProperty("gdprConsentGiven")]
        public bool GdprConsentGiven { get; set; }
    }
}