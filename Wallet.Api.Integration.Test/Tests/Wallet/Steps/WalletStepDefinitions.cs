using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Wallet.Api.Integration.Test.Extensions;
using Wallet.Api.Integration.Test.Setup;
using Wallet.Api.Integration.Test.Tests.Wallet.Steps.TestModels;
using Wallet.Contract.Command.Wallet;
using Wallet.Contract.Response.Wallet;

namespace Wallet.Api.Integration.Test.Tests.Wallet.Steps
{
    [Binding]
    public class WalletStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;
        private readonly WalletFixture walletFixture;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
 
        private IReadOnlyList<Domain.Entities.Wallet>? wallets; 

        public WalletStepDefinitions(
            ScenarioContext scenarioContext,
            WalletFixture walletFixture,
            TestApplicationFactoryFixture webApplicationFactory,
            IMapper mapper)
        {
            this.scenarioContext = scenarioContext;
            this.walletFixture = walletFixture;
            this.mapper = mapper;

            this.httpClient = webApplicationFactory.CreateHttpClient(
                services =>
                { 
                    services.AddScoped(_ => this.walletFixture.walletRepository);
                });
        }
        
        [Given("Wallets are")]
        public async Task GivenWalletsAre(Table table)
        {
            this.wallets = (IReadOnlyList<Domain.Entities.Wallet>)table.CreateSet<Domain.Entities.Wallet>();
            this.wallets.AssertNotNull();
            foreach (var wallet in wallets)
            {
                await this.walletFixture.walletRepository.CreateAsync(wallet);
            }
        }
 
        [When(@"POST ""/wallet/create/(.+)"" is called with parameters")]
        public async Task WhenCreateWalletCommandEndpointIsCalledWithParametersAsync(string userId, Table table)
        {
            CreateWalletCommand request = table.CreateInstance<CreateWalletCommand>();
            string endpoint = $"/wallet/create/{userId}";
            await this.httpClient.CallEndpointAsync<object>(HttpMethod.Post, endpoint, request, this.scenarioContext);
        }
        
        [When(@"GET ""/wallet/(.+)"" is called")]
        public async Task GetWalletByUserIdIsCalledAsync(string userId)
        {
            string endpoint = $"/wallet/{userId}";
            await this.httpClient.CallEndpointAsync<WalletResponse>(HttpMethod.Get, endpoint, null, this.scenarioContext);
        }
        
        [Then(@"Wallets should be")]
        public async Task ThenWalletsShouldBeAsync(Table table)
        {
            List<Domain.Entities.Wallet> walletResult  = (await this.walletFixture.walletRepositoryForTest.GetAllAsync()).OrderBy(x=>x.UserId).ToList();
            table.CompareToSet(this.mapper.Map<List<WalletTestModel>>(walletResult), true); 
        } 
        
        [Then(@"Get Wallet By User should be")]
        public void ThenGetWalletByUserShouldBeAsync(Table table)
        {
            var result = this.scenarioContext["ResponseData"].As<WalletResponse>();
            table.CompareToSet(this.mapper.Map<List<WalletTestModel>>(result.Items), true); 
        } 
    }
}