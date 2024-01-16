using MongoDB.Bson.Serialization;
using Wallet.Domain.Entities;

namespace Wallet.Data.Extensions;

internal static class MongoDbEntitiesClassMaps
{
    internal static void Initialize()
    {
        MapWallet();
    } 

    private static void MapWallet()
    {

        BsonClassMap.RegisterClassMap<TestEntity>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }); 
    }

}