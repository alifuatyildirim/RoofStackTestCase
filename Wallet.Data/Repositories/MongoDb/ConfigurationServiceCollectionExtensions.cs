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

    public static IServiceCollection ConfigureAndGetWithOptionName<TOptions>(this IServiceCollection services,
        out TOptions options)
        where TOptions : class
    {
        IConfigurationSection configurationSection =
            services.GetDefaultConfiguration().GetSection(typeof(TOptions).Name);
        options = configurationSection.Get<TOptions>();

        return services.Configure<TOptions>(configurationSection);
    }

    public static TOptions GetOptionWithName<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        IConfigurationSection configurationSection =
            services.GetDefaultConfiguration().GetSection(typeof(TOptions).Name);
        return configurationSection.Get<TOptions>();
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