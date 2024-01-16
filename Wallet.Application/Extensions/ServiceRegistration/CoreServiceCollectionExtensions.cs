using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Wallet.Application.Extensions.ServiceRegistration
{
    public static class CoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services.AddMapper();
        } 

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            Assembly mapperRegistryAssm = Assembly.GetAssembly(typeof(MapperRegistry))!;
            services.AddWalletMapper(assemblies: new[]
            {
                mapperRegistryAssm,
            });
            return services;
        }
        
        public static IServiceCollection AddWalletMapper(this IServiceCollection services, Action<TypeAdapterConfig>? options = null, params Assembly[] assemblies)
        {
            TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
            
            config.AllowImplicitDestinationInheritance = true;
            
            if (config.RuleMap.IsEmpty)
            {
                config.Scan(assemblies);
                options?.Invoke(config);
            }

            services
                .AddSingleton(config)
                .AddScoped<IMapper, ServiceMapper>();
 
            return services;
        }
 
    }
}
