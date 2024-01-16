using System.Reflection;
using Wallet.Api.Middlewares;
using Wallet.Application.Extensions.ServiceRegistration;
using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Mediator;
using Wallet.Data.Extensions;
using Wallet.Data.Repositories.MongoDb;

namespace Wallet.Api.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddSwaggerGen(c => c.SwaggerGenSetup(env.EnvironmentName, env.ApplicationName))
            .AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    public static IServiceCollection AddWalletServices(this IServiceCollection services, IConfiguration configuration)
    {
        IEnumerable<Assembly> assemblies = new List<Assembly>
        {
            Assembly.GetAssembly(typeof(IApplicationService))!
        };

        services.SetDefaultConfiguration(configuration)
            .AddCqrsMediator(assemblies)
            .AddApplicationServices()
            .AddCoreServices()
            .AddMongoDb();

        return services;
    }

    public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{env.ApplicationName} V1");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = $"{env.ApplicationName} ({env.EnvironmentName})";
            });
        return app;
    }

    public static IApplicationBuilder UseWalletMiddlewares(this IApplicationBuilder app)
    { 
        app.UseMiddleware<HttpLoggingMiddleware>()
            .UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    } 
}