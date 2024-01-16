using Microsoft.Extensions.DependencyInjection; 
using Pluralize.NET; 
using Wallet.Api.Integration.Test.Tests.Wallet;
using Wallet.Data.Repositories;
using Wallet.Data.Repositories.MongoDb;
using Wallet.Data.Repositories.MongoDb.Index;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Api.Integration.Test.Setup.ServiceRegistration
{
    public static class ServiceCollectionExtensions
    {  
        public static IServiceCollection AddMongoForTest(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseConnection, MongoDBTestDatabaseConnection>()
                .AddSingleton(typeof(IGenericRepository<,>), typeof(MongoDBGenericRepository<,>))
                .AddSingleton(typeof(IIndexKeysDefinitionBuilder<>), typeof(MongoDBIndexKeysDefinitionBuilder<>))
                .AddSingleton<IPluralize, Pluralizer>();

            services.Configure<MongoDBOptions>(x =>
            {
                ServiceProvider sp = services.BuildServiceProvider();
                Mongo2GoFixture? fixture = sp.GetService<Mongo2GoFixture>();
                x.Database = fixture?.MongoDatabaseName;
                x.ConnectionString = fixture?.MongoConnectionString;
            });

            services
                .AddSingleton<IWalletRepository, WalletRepository>()
                .AddSingleton<IWalletRepositoryForTest, WalletRepository>()
                .AddSingleton<IWalletTransactionRepository, WalletTransactionRepository>();
            return services;
        }

        public static IServiceCollection AddFixtures(this IServiceCollection services)
        {
            return services
                .AddScoped<WalletFixture>()
                .AddSingleton<Mongo2GoFixture>();
        }
 
    }
}