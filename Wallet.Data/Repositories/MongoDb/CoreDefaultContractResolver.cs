using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Wallet.Data.Repositories.MongoDb;

public class CoreDefaultContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

        if (jsonProperty.Writable)
        {
            return jsonProperty;
        }

        if (member is PropertyInfo property)
        { 
            jsonProperty.Writable = property.GetSetMethod(true) != null;
        }

        return jsonProperty;
    }
}