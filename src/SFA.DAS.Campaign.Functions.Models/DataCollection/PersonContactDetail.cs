using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class PersonContactDetail
    {
        [JsonProperty("Captured")]
        public string Captured { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("emailVerificationCompletion")]
        public string EmailVerificationCompleted { get; set; }
    }
}