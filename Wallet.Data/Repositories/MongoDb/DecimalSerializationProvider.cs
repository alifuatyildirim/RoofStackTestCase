using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Wallet.Data.Repositories.MongoDb;

public class DecimalSerializationProvider : IBsonSerializationProvider
{
    private static readonly DecimalSerializer DecimalSerializer = new DecimalSerializer(BsonType.Decimal128);
    private static readonly NullableSerializer<decimal> NullableSerializer = new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128));

    public IBsonSerializer? GetSerializer(Type type)
    {
        return type switch
        {
            _ when type == typeof(decimal) => DecimalSerializer,
            _ when type == typeof(decimal?) => NullableSerializer,
            _ => null,
        };
    }
}