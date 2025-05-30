﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace DAS.DigitalEngagement.Functions.DataCollection.Extensions;

[ExcludeFromCodeCoverage]
public static partial class ConfigureNServiceBusExtension
{
    public static class AzureRuleNameShortener
    {
        private const int AzureServiceBusRuleNameMaxLength = 50;

        public static string Shorten(Type arg)
        {
            var ruleName = arg.FullName;
            if (ruleName!.Length <= AzureServiceBusRuleNameMaxLength)
            {
                return ruleName;
            }

            using var md5 = MD5.Create();
            var bytes = System.Text.Encoding.Default.GetBytes(ruleName);
            var hash = md5.ComputeHash(bytes);
            var shortenedRuleName = new Guid(hash).ToString();

            return shortenedRuleName;
        }
    }
}
