using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Common.Abstraction;
using Wallet.Data.Repositories.MongoDb;

namespace Wallet.Data.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddMongoDBRepository()  
            .AddRepositories();
        
        MongoDbClassMaps.Initialize();
            
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Assembly assm = Assembly.GetExecutingAssembly();
        static bool Expression(Type type) => typeof(IRepository).IsAssignableFrom(type);

        foreach (Type v in assm.GetTypes().Where(type =>
                     !type.IsInterface && Expression(type) && type.GetInterfaces().Any(Expression)))
        {
            foreach (Type i in v.GetInterfaces().Where(Expression))
            {
                services.AddSingleton(i, v);
            }
        }

        return services;
    }
}