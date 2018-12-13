using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class PersonConsent
    {
        [JsonProperty("gdprConsentDeclared")]
        public string GdprConsentDeclared { get; set; }
        [JsonProperty("gdprConsentGiven")]
        public bool GdprConsentGiven { get; set; }
    }
}