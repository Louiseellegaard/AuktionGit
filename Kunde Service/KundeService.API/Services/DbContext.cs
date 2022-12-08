using MongoDB.Driver;

using KundeService.Models;

namespace KundeService.Services
{
	public interface IDbContext
	{
		IMongoCollection<Kunde> KundeCollection { get; }
	}

	public class DbContext : IDbContext
	{
		private ILogger<DbContext> _logger;
		public IMongoCollection<Kunde> KundeCollection { get; }

		public DbContext(ILogger<DbContext> logger)
		{
			_logger = logger;

			var _connectionString = "mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority";

			// Opretter en 'MongoClient' med forbindelse til MongoDB Atlas
			var _client = new MongoClient(_connectionString);

			// Henter auktions-databasen fra '_client'
			var _mongoDatabase = _client.GetDatabase("Auktiondb");

			// Henter kunde-collection fra '_mongoDatabase'
			KundeCollection = _mongoDatabase.GetCollection<Kunde>("Kunde");

			_logger.LogInformation("Forbundet til database {database}", _mongoDatabase);
			_logger.LogInformation("Benytter collection {collection}", KundeCollection);
		}
	}
}
