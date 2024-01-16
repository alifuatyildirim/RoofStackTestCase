using AdCodicem.SpecFlow.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Wallet.Api.Integration.Test.Setup.ServiceRegistration;
using Wallet.Api.Integration.Test.Setup.Substitution;
using Wallet.Application.Extensions.ServiceRegistration;
using Wallet.Data.Extensions;
using Wallet.Data.Repositories;
using Wallet.Data.Repositories.MongoDb;
using Wallet.Domain.Repositories;
using Xunit.Abstractions;

namespace Wallet.Api.Integration.Test.Setup
{
    public class TestServiceConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.SetDefaultConfiguration(Substitute.For<IConfiguration>()); 
            
            services
                .AddScoped<TestApplicationFactoryFixture>()
                .AddDelegated<ITestOutputHelper>()
                .AddSingleton(typeof(ILogger<>), typeof(SubstituteLogger<>))
                .AddMongoForTest()
                .AddCoreServices()
                .AddFixtures()
                .AddHttpContextAccessor();
        }
    }
}