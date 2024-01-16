using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Wallet.Api.Attributes;
using Wallet.Api.Extensions;
using Wallet.Api.Integration.Test.Setup.ServiceRegistration;
using Wallet.Application.Extensions.ServiceRegistration;
using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Mediator;
using Wallet.Data.Repositories.MongoDb;

namespace Wallet.Api.Integration.Test.Setup
{ 
    public class TestApplicationServerStartup
    { 
        public void ConfigureServices(IServiceCollection services)
        { 
            services.SetDefaultConfiguration(Substitute.For<IConfiguration>());

            services.AddControllers(config => config.Filters.Add(new FromBodyFilterAttribute()))
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = false;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;
                    options.ClientErrorMapping[404].Link =
                        "https://httpstatuses.com/404";
                });
            
            services.AddHttpContextAccessor();

            IEnumerable<Assembly> assemblies = new List<Assembly>
            {
                Assembly.GetAssembly(typeof(IApplicationService))!,
            };

            services.AddCqrsMediator(assemblies)
                .AddApplicationServices()
                .AddMongoForTest()
                .AddCoreServices();
 

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddApplicationPart(typeof(Wallet.Api.Startup).Assembly)
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
                
            app.UseWalletMiddlewares();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}