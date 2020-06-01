using System;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.DataCollection
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

        public Person MapFromUserData(UserData user, DateTime? creationDate = null)
        {
            if(!creationDate.HasValue)
            {
                creationDate = DateTime.UtcNow;
            }
            var creationDateString = creationDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            var isCreateUser = IsCreateUser(user);
            return new Person
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Enrolled = isCreateUser ? creationDateString : null,
                ContactDetail = new PersonContactDetail
                {
                    Captured = isCreateUser ? creationDateString : null,
                    EmailAddress = user.Email,
                    EmailVerificationCompleted = null,
                },
                Consent = new PersonConsent
                {
                    GdprConsentDeclared = creationDateString,
                    GdprConsentGiven = user.Consent
                },
                Cookie = isCreateUser ? new PersonCookie
                {
                    Captured = creationDateString,
                    CookieIdentifier = user.CookieId
                } : null,
                Route = isCreateUser ? new PersonRoute
                {
                    Captured = creationDateString,
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