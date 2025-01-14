using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Entities
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<Board> Boards => _database.GetCollection<Board>("Boards");
        public IMongoCollection<Task1> Tasks => _database.GetCollection<Task1>("Tasks");
    }
}
