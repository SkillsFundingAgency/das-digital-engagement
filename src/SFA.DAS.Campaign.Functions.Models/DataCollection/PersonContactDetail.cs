using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class PersonContactDetail
    {
        [JsonProperty("Captured", NullValueHandling = NullValueHandling.Ignore)]
        public string Captured { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("emailVerificationCompletion", NullValueHandling = NullValueHandling.Ignore)]
        public string EmailVerificationCompleted { get; set; }
    }
}