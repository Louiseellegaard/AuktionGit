using MongoDB.Driver;

using BudService.Models;

namespace BudService.Services
{
    public interface IDbContext
    {
        IMongoCollection<Bud> BudCollection { get; }
    }

    public class DbContext : IDbContext
    {
		private ILogger<DbContext> _logger;
		public IMongoCollection<Bud> BudCollection { get; }

		public DbContext(ILogger<DbContext> logger)
		{
			_logger = logger;

			var _connectionString = "mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority";

            // Opretter en 'MongoClient' med forbindelse til MongoDB Atlas
            var _client = new MongoClient(_connectionString);

			// Henter auktions-databasen fra '_client'
			var _mongoDatabase = _client.GetDatabase("Auktiondb");

			// Henter bud-collection fra '_mongoDatabase'
			BudCollection = _mongoDatabase.GetCollection<Bud>("Bud");

			_logger.LogInformation("Forbundet til database {database}", _mongoDatabase);
			_logger.LogInformation("Benytter collection {collection}", BudCollection);
		}
    }
}
