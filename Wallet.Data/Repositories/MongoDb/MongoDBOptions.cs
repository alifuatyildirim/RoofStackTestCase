namespace Wallet.Data.Repositories.MongoDb;

public class MongoDBOptions
{
    public string? ConnectionString { get; set; }

    public string? Database { get; set; }

    public bool? ActivateSubscriber { get; set; }
}