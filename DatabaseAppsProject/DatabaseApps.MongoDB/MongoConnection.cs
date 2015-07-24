namespace MongoDB
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    public class MongoConnection
    {
        const string connectionString = "mongodb://localhost";
        private static MongoClient client = new MongoClient(connectionString);
        MongoServer server = client.GetServer();

        public MongoConnection(string databaseName)
        {
            this.Database = server.GetDatabase(databaseName);
            this.Collections = new Dictionary<string, MongoCollection>();
        }

        private MongoDatabase Database { get; set; }
        public Dictionary<string, MongoCollection> Collections { get; private set; }

        public void CreateCollection<T>(string collectionName)
        {
            var collection = this.Database.GetCollection<T>(collectionName);
            this.Collections.Add(collectionName, collection);
        }
    }
}