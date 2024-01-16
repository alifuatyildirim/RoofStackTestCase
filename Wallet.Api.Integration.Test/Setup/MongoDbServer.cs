using EphemeralMongo;

namespace Wallet.Api.Integration.Test.Setup
{
    public static class MongoDbServer
    {
        private static IMongoRunner? mongoDbRunner;

        public static string ConnectionString { get; private set; } = string.Empty;

        public static void Start()
        {
            // All properties below are optional. The whole "options" instance is optional too!
            var options = new MongoRunnerOptions
            {
                UseSingleNodeReplicaSet = true, // Default: false
                //StandardOuputLogger = line => Console.WriteLine(line), // Default: null
                //StandardErrorLogger = line => Console.WriteLine(line), // Default: null
                //DataDirectory = "/path/to/data/", // Default: null
                //BinaryDirectory = "/path/to/mongo/bin/", // Default: null
                //ConnectionTimeout = TimeSpan.FromSeconds(10), // Default: 30 seconds
                //ReplicaSetSetupTimeout = TimeSpan.FromSeconds(5), // Default: 10 seconds
                //AdditionalArguments = "--quiet", // Default: null
                //MongoPort = 27017, // Default: random available port
            };

            mongoDbRunner = MongoRunner.Run(options);
            
            //mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true, singleNodeReplSetWaitTimeout: 10, logger: NullLogger<MongoDbRunner>.Instance);
            
            ConnectionString = mongoDbRunner.ConnectionString;
        }
        
        public static void End()
        {
            mongoDbRunner?.Dispose();
        }
    }
}