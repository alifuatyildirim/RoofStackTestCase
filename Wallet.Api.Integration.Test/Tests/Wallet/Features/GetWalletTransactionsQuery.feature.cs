﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Wallet.Api.Integration.Test.Tests.Wallet.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GetWalletTransactionsQueryFeature : object, Xunit.IClassFixture<GetWalletTransactionsQueryFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "GetWalletTransactionsQuery.feature"
#line hidden
        
        public GetWalletTransactionsQueryFeature(GetWalletTransactionsQueryFeature.FixtureData fixtureData, Wallet_Api_Integration_Test_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Tests/Wallet/Features", "GetWalletTransactionsQuery", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Retrieve Wallet Transaction Information for a User with GET Request")]
        [Xunit.TraitAttribute("FeatureTitle", "GetWalletTransactionsQuery")]
        [Xunit.TraitAttribute("Description", "Retrieve Wallet Transaction Information for a User with GET Request")]
        public virtual void RetrieveWalletTransactionInformationForAUserWithGETRequest()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Wallet Transaction Information for a User with GET Request", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 3
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "UserId",
                            "CurrencyCode",
                            "Limit",
                            "CreatedDate"});
                table11.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "11111111-1111-1111-1111-111111111111",
                            "TRY",
                            "250",
                            "2024-01-13"});
                table11.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999991",
                            "11111111-1111-1111-1111-111111111111",
                            "Dolar",
                            "100",
                            "2024-01-13"});
                table11.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999992",
                            "22222222-2222-2222-2222-222222222222",
                            "TRY",
                            "1000",
                            "2024-01-13"});
#line 4
        testRunner.Given("Wallets are", ((string)(null)), table11, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                            "WalletId",
                            "CurrencyCode",
                            "Amount",
                            "TransactionType",
                            "CreatedBy"});
                table12.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "TRY",
                            "100.0",
                            "Deposit",
                            "11111111-1111-1111-1111-111111111111"});
                table12.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "TRY",
                            "200.0",
                            "Deposit",
                            "11111111-1111-1111-1111-111111111111"});
                table12.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "TRY",
                            "50.0",
                            "WithDraw",
                            "11111111-1111-1111-1111-111111111111"});
#line 9
        testRunner.Given("Wallet Transactions are", ((string)(null)), table12, "Given ");
#line hidden
#line 14
        testRunner.When("GET \"/walletTransaction/getAll/99999999-9999-9999-9999-999999999990\" is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 15
        testRunner.Then("Http status code should be 200 and Message should be \"\" and error code should be " +
                        "\"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                            "WalletId",
                            "Amount",
                            "TransactionType",
                            "CreatedBy"});
                table13.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "100.0",
                            "Deposit",
                            "11111111-1111-1111-1111-111111111111"});
                table13.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "200.0",
                            "Deposit",
                            "11111111-1111-1111-1111-111111111111"});
                table13.AddRow(new string[] {
                            "99999999-9999-9999-9999-999999999990",
                            "50.0",
                            "WithDraw",
                            "11111111-1111-1111-1111-111111111111"});
#line 16
        testRunner.Then("Get Wallet Transactions should be", ((string)(null)), table13, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GetWalletTransactionsQueryFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GetWalletTransactionsQueryFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
