using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 
using Wallet.Data.Repositories.MongoDb;
using Xunit.Abstractions;

namespace Wallet.Api.Integration.Test.Setup
{
    public class TestApplicationFactoryFixture : WebApplicationFactory<TestApplicationServerStartup>
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly MongoDBOptions mongoDbOptions;

        public TestApplicationFactoryFixture(
            ITestOutputHelper testOutputHelper,
            IOptions<MongoDBOptions> options)
        {
            this.testOutputHelper = testOutputHelper;
            this.mongoDbOptions = options.Value;
        }
    
        internal string? TestEnvironment { get; set; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder() 
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddXUnit(this.testOutputHelper);
                })
                .ConfigureWebHostDefaults(builder => builder.UseStartup<TestApplicationServerStartup>().UseTestServer());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Don't run IHostedServices when running as a test
            //builder.ConfigureTestServices((services) =>
            //{
            //    services.RemoveAll(typeof(IHostedService));
            //});
        }

        public HttpClient CreateHttpClient(Action<IServiceCollection> configureServices, Action<IServiceCollection>? configureTestServices = null)
        {   
            return this.WithWebHostBuilder(builder =>
                {
                    if (!string.IsNullOrEmpty(this.TestEnvironment))
                    {
                        builder.UseEnvironment(this.TestEnvironment);
                    }
                
                    builder
                        .ConfigureServices(services => configureServices(services))
                        .ConfigureTestServices(services =>
                        {
                            configureTestServices?.Invoke(services);

                            services.Configure<MongoDBOptions>(options =>
                            {
                                options.Database = this.mongoDbOptions.Database;
                                options.ConnectionString = this.mongoDbOptions.ConnectionString;
                            });
                        });
                })
                .CreateDefaultClient();
        }
    }
}
