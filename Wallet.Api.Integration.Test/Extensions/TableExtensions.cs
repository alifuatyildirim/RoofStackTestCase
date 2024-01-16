using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Wallet.Api.Integration.Test.Extensions
{
    public static class TableExtensions
    {
        public static string ConvertToQueryString<T>(this Table table)
            where T : notnull
        {
            T queryParams = table.CreateInstance<T>();
            Dictionary<string, string?> query = queryParams.GetType().GetProperties()
                .Where(x => table.Header.Contains(x.Name))
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(queryParams, null)?.ToString());

            using var queryContent = new FormUrlEncodedContent(query!);
            return queryContent.ReadAsStringAsync().Result;
        }   
    }
}