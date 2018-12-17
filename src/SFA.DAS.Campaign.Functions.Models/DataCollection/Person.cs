using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class Person
    {
        [JsonProperty("firstName",NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; internal set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; internal set; }
        [JsonProperty("enrolled", NullValueHandling = NullValueHandling.Ignore)]
        public string Enrolled { get; internal set; }
        [JsonProperty("Consent")]
        public PersonConsent Consent { get; internal set; }
        [JsonProperty("Cookie", NullValueHandling = NullValueHandling.Ignore)]
        public PersonCookie Cookie { get; internal set; }
        [JsonProperty("Route", NullValueHandling = NullValueHandling.Ignore)]
        public PersonRoute Route { get; internal set; }
        [JsonProperty("ContactDetail")]
        public PersonContactDetail ContactDetail { get; internal set; }

        public Person MapFromUserData(UserData user)
        {
            var isCreateUser = IsCreateUser(user);
            return new Person
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Enrolled = isCreateUser ? DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") : null,
                ContactDetail = new PersonContactDetail
                {
                    Captured = isCreateUser ? DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") : null,
                    EmailAddress = user.Email,
                    EmailVerificationCompleted = null,
                },
                Consent = new PersonConsent
                {
                    GdprConsentDeclared = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    GdprConsentGiven = user.Consent
                },
                Cookie = isCreateUser ? new PersonCookie
                {
                    Captured = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    CookieIdentifier = user.CookieId
                } : null,
                Route = isCreateUser ? new PersonRoute
                {
                    Captured = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    RouteIdentifier = user.RouteId
                } : null
            };
        }

        private bool IsCreateUser(UserData user)
        {
            return !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
        }
    }
}