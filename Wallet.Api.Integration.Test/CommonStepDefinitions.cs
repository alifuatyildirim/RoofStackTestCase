#nullable enable
using System.Net;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;
using Wallet.Api.Integration.Test.Extensions;
using Wallet.Api.Integration.Test.Setup;
using Wallet.Api.Integration.Test.Setup.CustomValueHandler;
using Wallet.Data.Extensions;

namespace Wallet.Api.Integration.Test
{
    [Binding]
    public class CommonStepDefinitions
    {
        private readonly ScenarioContext scenarioContext; 

        public CommonStepDefinitions(
            ScenarioContext scenarioContext, 
            TestApplicationFactoryFixture webApplicationFactory)
        {
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void RegisterValueMappings()
        {
            Service.Instance.ValueComparers.Register(new NullValueComparer("<null>"));
            Service.Instance.ValueComparers.Register(new CurrentDateTimeValueHandler());

            Service.Instance.ValueRetrievers.Register(new NullValueRetriever("<null>"));
            Service.Instance.ValueRetrievers.Register(new CurrentDateTimeValueHandler());
           
            ScenarioDateTimeExtensions.SetScenarioDateTime();
            
            MongoDbServer.End();
            MongoDbServer.Start();
            MongoDbClassMaps.Initialize();
        }
        
        [AfterTestRun]
        public static void CleanUp()
        {
            MongoDbServer.End();
        }
        
        [Then(@"Http status code should be (.+) and Message should be ""(.*)"" and error code should be ""(.*)""")]
        public void ThenHttpStatusCodeShouldBeAndMessageShouldBe(HttpStatusCode httpStatusCode, string? message, string errorCode)
        {
            this.ControlResponse(httpStatusCode, message, errorCode);
        }
        
        private void ControlResponse(HttpStatusCode httpStatusCode, string? message, string? errorCode)
        {
            this.scenarioContext["HttpStatusCode"].As<HttpStatusCode>().Should().Be(httpStatusCode);

            message = string.IsNullOrEmpty(message) ? null : message;
            this.scenarioContext["ResponseMessage"].As<string?>().Should().Be(message);

            errorCode = string.IsNullOrEmpty(errorCode) ? null : errorCode;
            this.scenarioContext["ResponseErrorCode"].As<string?>().Should().Be(errorCode);
        }
    }
}
