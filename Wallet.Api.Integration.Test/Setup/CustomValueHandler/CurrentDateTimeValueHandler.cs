using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Wallet.Api.Integration.Test.Extensions;
using TechTalk.SpecFlow.Assist;

namespace Wallet.Api.Integration.Test.Setup.CustomValueHandler
{
    public class CurrentDateTimeValueHandler : IValueComparer, IValueRetriever
    {
        internal const string ComparerKey = "CURRENT_DATETIME";
        private const string DateTimeRegexPattern = @"CURRENT_DATETIME(?<local>(_LOCAL)){0,1}(?<dateonly>(_ASDATE)){0,1}(\s*)(?<amount>[-\+]\d+){0,1}(?<unit>(ms|s|m|h|d|M|y)){0,1}(?<format>(.*))";

        private readonly Regex dateTimeRegex = new Regex(DateTimeRegexPattern);

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return this.dateTimeRegex.Match(keyValuePair.Value).Success;
        }

        public object? Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if (keyValuePair.Value is "<null>" or "null")
            {
                return null;
            }
            
            Match match = this.dateTimeRegex.Match(keyValuePair.Value);

            if (!match.Success)
            {
                return DateTime.Parse(keyValuePair.Value, CultureInfo.InvariantCulture);
            }

            return match.ToDateTime();
        }
        
        public bool CanCompare(object actualValue)
        {
            return actualValue is DateTime;
        }

        public bool Compare(string expectedValue, object actualValue)
        {
            var actualDateTime = (DateTime)actualValue;

            Match match = this.dateTimeRegex.Match(expectedValue);

            if (!match.Success)
            {
                return false;
            }

            DateTime expectedDateTime = match.ToDateTime();

            string? format = match.Groups["format"].Value;
            if (!string.IsNullOrEmpty(format.Trim()))
            {
                return expectedDateTime.ToString(format, CultureInfo.InvariantCulture) == actualDateTime.ToString(format, CultureInfo.InvariantCulture);
            }

            return expectedDateTime == (DateTime)actualValue;
        }
    }
}
