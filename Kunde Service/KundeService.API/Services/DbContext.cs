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
		public DbContext()
		{
			var connectionString = "mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority";

			var client = new MongoClient(connectionString);

			var _mongoDatabase = client.GetDatabase("Auktiondb");

			KundeCollection = _mongoDatabase.GetCollection<Kunde>("Kunde");
		}

		public IMongoCollection<Kunde> KundeCollection { get; }
	}
}
