using MongoDB.Driver;

using AuktionService.Models;

namespace AuktionService.Services
{
    public interface IDbContext
    {
        IMongoCollection<Auktion> Collection { get; }
    }

    public class DbContext : IDbContext
    {
		private ILogger<DbContext> _logger;
		private IConfiguration _config;
		public IMongoCollection<Auktion> Collection { get; }

		public DbContext(ILogger<DbContext> logger, IConfiguration config)
		{
			_logger = logger;
			_config = config;

			// Henter ConnectionString fra environment i 'docker-compose.yml'-filen
			var _connectionString = _config["ConnectionString"];

            // Opretter en 'MongoClient' med forbindelse til MongoDB Atlas
            var _mongoClient = new MongoClient(_connectionString);

			// Henter database fra environment i docker-compose
			var _mongoDatabase = _mongoClient.GetDatabase(_config["Database"]);

			// Henter collection fra environment i docker-compose
			Collection = _mongoDatabase.GetCollection<Auktion>(_config["Collection"]);

			_logger.LogInformation("Forbundet til database {database}", _mongoDatabase.DatabaseNamespace.DatabaseName);
			_logger.LogInformation("Benytter collection {collection}", Collection.CollectionNamespace.CollectionName);
		}
    }
}
