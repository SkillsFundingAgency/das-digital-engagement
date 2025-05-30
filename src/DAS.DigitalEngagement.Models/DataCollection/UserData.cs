﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DAS.DigitalEngagement.Models.DataCollection
{
    public class UserData
    {
        public long EmployerAccountId { get; set; }
        public int StageCompleted { get; set; }
        public string StageCompletedText { get; set; }
        public int TotalStages { get; set; }
        public DateTime DateOfEvent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UkEmployerSize { get; set; }
        public string PrimaryIndustry { get; set; }
        public string PrimaryLocation { get; set; }
        public DateTime? AppsgovSignUpDate { get; set; }
        public string PersonOrigin { get; set; }
        public string EncodedEmail { get; set; }
        public bool Consent { get; set; }
        public bool IncludeInUR { get; set; }
        public string CookieId { get; set; }
        public string RouteId { get; set; }
        /*
        * Email validation taken from https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        */
        private bool DomainInvalid { get; set; }
        public string MarketoCookieId { get; set; }

        public bool IsValidEmail()
        {
            var strIn = Email;
            if (string.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (DomainInvalid)
            {
                return false;
            }

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                DomainInvalid = true;
            }

            return match.Groups[1].Value + domainName;
        }
    }
}
