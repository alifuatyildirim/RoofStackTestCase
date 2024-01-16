using System;
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
using Wallet.Contract.Command.WalletTransaction;
using Wallet.Contract.Response.Wallet;
using Wallet.Contract.Response.WalletTransaction;

namespace Wallet.Api.Integration.Test.Tests.Wallet.Steps
{
    [Binding]
    public class WalletTransactionStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;
        private readonly WalletFixture walletFixture;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
 
        private IReadOnlyList<Domain.Entities.WalletTransaction>? walletTransactions; 

        public WalletTransactionStepDefinitions(
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
        
        [Given("Wallet Transactions are")]
        public async Task GivenWalletTransactionsAre(Table table)
        {
            this.walletTransactions = (IReadOnlyList<Domain.Entities.WalletTransaction>)table.CreateSet<Domain.Entities.WalletTransaction>();
            this.walletTransactions.AssertNotNull();
            foreach (var wallet in walletTransactions)
            {
                await this.walletFixture.walletTransactionRepository.CreateAsync(wallet);
            }
        }
 
        [When(@"POST ""/walletTransaction/create/(.+)"" is called with parameters")]
        public async Task WhenCreateWalletTransactionCommandEndpointIsCalledWithParametersAsync(string walletId, Table table)
        {
            CreateWalletTransactionCommandTestModel request = table.CreateInstance<CreateWalletTransactionCommandTestModel>();
            string endpoint = $"/walletTransaction/create/{walletId}";
            await this.httpClient.CallEndpointAsync<object>(HttpMethod.Post, endpoint, request, this.scenarioContext);
        }
        
        [When(@"GET ""/walletTransaction/getAll/(.+)"" is called")]
        public async Task GetWalletTransactionsIsCalledAsync(string walletId)
        {
            string endpoint = $"/walletTransaction/getAll/{walletId}";
            await this.httpClient.CallEndpointAsync<WalletTransactionResponse>(HttpMethod.Get, endpoint, null, this.scenarioContext);
        }
        
        [Then(@"Wallet By User should be for user (.+)")]
        public async Task ThenWalletsShouldBeAsync(Guid userId, Table table)
        {
            Domain.Entities.Wallet walletResult =
                (await this.walletFixture.walletRepository.GetByUserIdAsync(userId)).FirstOrDefault();
            table.CompareToInstance(this.mapper.Map<WalletTestModel>(walletResult)); 
        } 
        
        [Then(@"Wallet Transaction should be for wallet (.+)")]
        public async Task ThenWalletTransactionsShouldBeForUserAsync(Guid walletId, Table table)
        {
            IReadOnlyCollection<Domain.Entities.WalletTransaction> walletTransactionResult =
                (await this.walletFixture.walletTransactionRepository.GetAllTransactions(walletId)).Items;
            table.CompareToSet(this.mapper.Map<List<WalletTransactionsTestModel>>(walletTransactionResult)); 
        } 
        
        [Then(@"Get Wallet Transactions should be")]
        public void ThenGetWalletTransactionsShouldBeAsync(Table table)
        {
            var result = this.scenarioContext["ResponseData"].As<WalletTransactionResponse>();
            table.CompareToSet(this.mapper.Map<List<WalletTransactionsTestModel>>(result.Rows), true); 
        } 
    }
}