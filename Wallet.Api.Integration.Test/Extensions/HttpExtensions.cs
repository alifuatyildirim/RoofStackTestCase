using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Wallet.Api.Integration.Test.Setup.Contract;
using Wallet.Contract;

namespace Wallet.Api.Integration.Test.Extensions
{
    public static class HttpExtensions
    {
        public static string GetHeader(this HttpRequest request, string key)
        {
            StringValues result = request.Headers.FirstOrDefault(x => x.Key == key).Value;
            return result.FirstOrDefault() ?? string.Empty;
        }

        public static T GetResponse<T>(this HttpResponseMessage httpResponseMessage)
            where T : new()
        {
            string? jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonContent) ?? new T();
        }

        public static HttpClient AddHeader(this HttpClient httpClient, string key, string value)
        {
            httpClient!.DefaultRequestHeaders.Add(key, value);
            return httpClient;
        }

        public static async Task<TestHttpResult<ApiResponse<T>>> GetApiResponseAsync<T>(this HttpClient httpClient, string path)
            where T : class
        {
            HttpResponseMessage response = await httpClient.GetAsync(path);
            return response.GetHttpResult<T>();
        }

        public static TestHttpResult<ApiResponse<T>> GetHttpResult<T>(this HttpResponseMessage httpResponseMessage)
            where T : class
        {
            return new TestHttpResult<ApiResponse<T>>(httpResponseMessage.GetResponse<ApiResponse<T>>(), httpResponseMessage.StatusCode);
        }

        public static async Task CallEndpointAsync<TData>(this HttpClient httpClient, HttpMethod httpMethod, string endpoint, object? requestObject, ScenarioContext scenarioContext)
            where TData : class
        {
            httpClient.AddHeaders(scenarioContext.GetHttpContext());
            TestHttpResult<ApiResponse<TData>> result = await httpClient.SendRequestAndReturnAsync<TData>(httpMethod, endpoint, requestObject);
            scenarioContext["HttpStatusCode"] = result.HttpStatusCode;
            scenarioContext["ResponseMessage"] = result.Response.Message;
            scenarioContext["ResponseData"] = result.Response.Data;
            scenarioContext["ResponseErrorCode"] = result.Response.ErrorCode;
        }

        private static void AddHeaders(this HttpClient httpClient, DefaultHttpContext httpContext)
        {
            httpClient.DefaultRequestHeaders.Clear();
            foreach (KeyValuePair<string, StringValues> header in httpContext.Request.Headers)
            {
                httpClient.AddHeader(header.Key, header.Value);
            }
        }

        public static async Task CallEndpointForFileAsync<T>(
            this HttpClient httpClient,
            HttpMethod httpMethod,
            string endpoint,
            object? requestObject,
            ScenarioContext scenarioContext)
            where T : class
        {
            httpClient.AddHeaders(scenarioContext.GetHttpContext());
            HttpResponseMessage result = await httpClient.SendRequestAsync(httpMethod, endpoint, requestObject); 

            var response = new TestHttpResult<ApiResponse<T>>(result.GetResponse<ApiResponse<T>>(), result.StatusCode);
            scenarioContext["HttpStatusCode"] = response.HttpStatusCode;
            scenarioContext["ResponseMessage"] = response.Response.Message;
            scenarioContext["ResponseData"] = response.Response.Data;
            scenarioContext["ResponseErrorCode"] = response.Response.ErrorCode;
        }

        public static async Task<TestHttpResult<ApiResponse<T>>> SendRequestAndReturnAsync<T>(this HttpClient httpClient, HttpMethod method, string path, object? requestObject)
            where T : class
        {
            HttpResponseMessage response = await httpClient.SendRequestAsync(method, path, requestObject);
            return new TestHttpResult<ApiResponse<T>>(response.GetResponse<ApiResponse<T>>(), response.StatusCode);
        }

        private static async Task<HttpResponseMessage> SendRequestAsync(this HttpClient httpClient, HttpMethod method, string path, object? requestObject)
        {
            using var httpRequestMessage = new HttpRequestMessage(method, path);
            httpRequestMessage.Content = await GetHttpContentAsync(requestObject);
            
            return await httpClient.SendAsync(httpRequestMessage);
        }
        
        private static async Task<HttpContent> GetHttpContentAsync(object? requestObject)
        {
            return new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
        }

        public static DefaultHttpContext GetHttpContext(this ScenarioContext scenarioContext)
        {
            if (scenarioContext.ContainsKey("HttpContext"))
            {
                return scenarioContext["HttpContext"].As<DefaultHttpContext>();
            }

            scenarioContext["HttpContext"] = new DefaultHttpContext();
            return scenarioContext["HttpContext"].As<DefaultHttpContext>();
        }
        
        private static async Task<byte[]> GetFileContentAsync(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);  
            var binaryReader = new BinaryReader(fs);  
            long byteLength = new FileInfo(filePath).Length;  
            byte[] fileContentArray = binaryReader.ReadBytes((int)byteLength);  
                
            fs.Close();  
            await fs.DisposeAsync();  
            binaryReader.Close();

            return fileContentArray;
        }
    }
}
