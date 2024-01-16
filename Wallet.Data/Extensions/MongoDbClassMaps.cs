using MongoDB.Bson.Serialization;
using Wallet.Data.Repositories.MongoDb;

namespace Wallet.Data.Extensions;

public static class MongoDbClassMaps
{
    public static void Initialize()
    {
        MongoDbEntitiesClassMaps.Initialize(); 
        AddSerializationProviders();
    } 
    private static void AddSerializationProviders()
    {
        BsonSerializer.RegisterSerializationProvider(new DecimalSerializationProvider());
    }
}