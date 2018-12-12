using System;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Functions.Models.DataCollection
{
    public class Person
    {
        [JsonProperty("firstName")]
        public string FirstName { get; internal set; }
        [JsonProperty("lastName")]
        public string LastName { get; internal set; }
        [JsonProperty("enrolled")]
        public string Enrolled { get; internal set; }
        [JsonProperty("Consent")]
        public PersonConsent Consent { get; internal set; }
        [JsonProperty("Cookie")]
        public PersonCookie Cookie { get; internal set; }
        [JsonProperty("Route")]
        public PersonRoute Route { get; internal set; }
        [JsonProperty("ContactDetail")]
        public PersonContactDetail ContactDetail { get; internal set; }

        public Person MapFromUserData(UserData user)
        {
            return new Person
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Enrolled = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                ContactDetail = new PersonContactDetail
                {
                    Captured = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    EmailAddress = user.Email,
                    EmailVerificationCompleted = null,
                },
                Consent = new PersonConsent
                {
                    GdprConsentDeclared = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    GdprConsentGiven = user.Consent
                },
                Cookie = new PersonCookie
                {
                    Captured = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    CookieIdentifier = user.CookieId
                },
                Route = new PersonRoute
                {
                    Captured = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                    RouteIdentifier = user.RouteId
                }
            };
        }
    }
}