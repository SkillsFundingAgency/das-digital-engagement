using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class WiredPlusUser
    {
        [JsonProperty("first_name")]
        public string FirstName { get; internal set; }
        [JsonProperty("last_name")]
        public string LastName { get; internal set; }
        [JsonProperty("email")]
        public string Email { get; internal set; }
        [JsonProperty("custom[encoded_email]")]
        public string EncodedEmail { get; internal set; }
        public bool Consent { get; internal set; }
        [JsonProperty("custom[cookie_id]")]
        public string CookieId { get; internal set; }
        [JsonProperty("custom[route_id]")]
        public string RouteId { get; internal set; }

        public WiredPlusUser MapFromUserData(UserData user)
        {
            return new WiredPlusUser
            {
                Email = user.Email,
                Consent = user.Consent,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RouteId = user.RouteId,
                CookieId = user.CookieId,
                EncodedEmail = user.EncodedEmail
            };
        }
    }
}
