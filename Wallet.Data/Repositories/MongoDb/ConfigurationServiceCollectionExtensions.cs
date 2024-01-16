using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Wallet.Data.Repositories.MongoDb;

public static class ConfigurationServiceCollectionExtensions
{
    private static readonly ConcurrentDictionary<IServiceCollection, IConfiguration> Configurations =
        new ConcurrentDictionary<IServiceCollection, IConfiguration>();

    public static IServiceCollection SetDefaultConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        Configurations[services] = configuration;
        return services;
    }

    public static IConfiguration GetDefaultConfiguration(this IServiceCollection services)
    {
        if (Configurations.TryGetValue(services, out IConfiguration? configuration))
        {
            return configuration;
        }

        throw new InvalidOperationException($"{nameof(SetDefaultConfiguration)} is not called!");
    }

    public static IServiceCollection ConfigureWithOptionName<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        return services.Configure<TOptions>(services.GetDefaultConfiguration().GetSection(typeof(TOptions).Name));
    }
    public static IServiceCollection AddNewtonsoftJsonCoreSettings(this IServiceCollection services)
    {
        var settings = new JsonSerializerSettings();

        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.ContractResolver = new CoreDefaultContractResolver();

        JsonConvert.DefaultSettings = () => settings;

        return services;
    }
}