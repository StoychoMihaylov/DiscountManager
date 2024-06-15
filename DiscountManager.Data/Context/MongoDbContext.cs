namespace DiscountManager.Data.Context
{
    using DiscountManager.Data.Documents;
    using DiscountManager.Models.Options;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class MongoDbContext
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbOptions> dbOptions)
        {
            var settings = dbOptions.Value;
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
            CreateIndex();
        }

        public IMongoClient Client => _client;
        public IMongoDatabase Database => _database;

        private void CreateIndex()
        {
            var codeCollection = _database.GetCollection<DiscountCodeDocument>(nameof(DiscountCodeDocument));
            var indexKeysDefinition = Builders<DiscountCodeDocument>.IndexKeys.Ascending(code => code.Code);
            var indexOptions = new CreateIndexOptions { Unique = true };
            codeCollection.Indexes.CreateOne(new CreateIndexModel<DiscountCodeDocument>(indexKeysDefinition, indexOptions));
        }
    }
}
