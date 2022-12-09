using MongoDB.Driver;

using AuktionService.Models;

namespace AuktionService.Services
{
    public interface IDbContext
    {
        IMongoCollection<Auktion> AuktionCollection { get; }
    }

    public class DbContext : IDbContext
    {
		private ILogger<DbContext> _logger;
		public IMongoCollection<Auktion> AuktionCollection { get; }

		public DbContext(ILogger<DbContext> logger)
		{
			_logger = logger;

			var _connectionString = "mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority";

            // Opretter en 'MongoClient' med forbindelse til MongoDB Atlas
            var _client = new MongoClient(_connectionString);

			// Henter auktions-databasen fra '_client'
			var _mongoDatabase = _client.GetDatabase("Auktiondb");

			// Henter bud-collection fra '_mongoDatabase'
			AuktionCollection = _mongoDatabase.GetCollection<Auktion>("Auktion");

			_logger.LogInformation("Forbundet til database {database}", _mongoDatabase);
			_logger.LogInformation("Benytter collection {collection}", AuktionCollection);
		}
    }
}
