using System;
using System.Collections.Generic;
using System.Text;
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
        public DateTime Enrolled { get; internal set; }
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
                ContactDetail = new PersonContactDetail
                {
                    EmailAddress = user.Email
                },
                Consent = new PersonConsent
                {
                    GdprConsentGiven = user.Consent
                },
                Cookie = new PersonCookie
                {
                    CookieIdentifier = user.CookieId
                },
                Route = new PersonRoute
                {
                    RouteIdentifier = user.RouteId
                }
            };
        }
    }
}