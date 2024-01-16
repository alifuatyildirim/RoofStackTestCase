#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions; 
using Wallet.Api.Integration.Test.Extensions;
using TechTalk.SpecFlow;
using Wallet.Api.Integration.Test.Setup; 

namespace Wallet.Api.Integration.Test.Tests.Common.Steps;

[Binding]
public class CommonSteps
{
    private readonly ScenarioContext scenarioContext;
    private readonly TestApplicationFactoryFixture webApplicationFactory; 
    private HttpClient? httpClient;

    public CommonSteps(ScenarioContext scenarioContext, TestApplicationFactoryFixture webApplicationFactory)
    {
        this.scenarioContext = scenarioContext;
        this.webApplicationFactory = webApplicationFactory; 
    }

    [Given("Test environment is (.*)")]
    public void GivenTestEnvironmentIs(string environmentName)
    {
        this.webApplicationFactory.TestEnvironment = environmentName; 
    }

    [When(@"""(.*)"" ""(.*)"" is called")]
    public async Task WhenDummyEndpointCalledAsync(string method, string endpoint)
    {
        endpoint.Should().NotBeNullOrEmpty();
        method.Should().NotBeNullOrEmpty();
        this.httpClient.AssertNotNull();
        await this.httpClient.CallEndpointAsync<object>(new HttpMethod(method), endpoint, null, this.scenarioContext);
    }
        
    [When(@"POST ""(.*)"" is called with item key ""(.+)"" and with parameters")]
    public async Task WhenDummyPostEndpointCalledAsync(string endpoint, string itemKey, Table parameters)
    {
        endpoint.Should().NotBeNullOrEmpty();
        IReadOnlyCollection<string> bodyData = parameters.Rows.Select(r => r[itemKey]).ToList();

        this.httpClient.AssertNotNull();
        await this.httpClient.CallEndpointAsync<object>(HttpMethod.Post, endpoint, bodyData, this.scenarioContext);
    }

    [When(@"PATCH ""(.*)"" is called with item key ""(.+)"" and with parameters")]
    public async Task WhenDummyPatchEndpointCalledAsync(string endpoint, string itemKey, Table parameters)
    {
        endpoint.Should().NotBeNullOrEmpty();
        IReadOnlyCollection<string> bodyData = parameters.Rows.Select(r => r[itemKey]).ToList();

        this.httpClient.AssertNotNull();
        await this.httpClient.CallEndpointAsync<object>(HttpMethod.Patch, endpoint, bodyData, this.scenarioContext);
    }
}