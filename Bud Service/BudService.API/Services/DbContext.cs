using MongoDB;
using MongoDB.Driver;
using System;
using BudService.Models;

namespace BudService.Services
{
    public interface IDbContext
    {
        IMongoCollection<Bud> BudCollection { get; }
    }

    public class DbContext : IDbContext
    {
        public DbContext()
        {
            var connectionString = "mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority";

            // Opretter en MongoDB-client med forbindelse til MongoDB Atlas
            var client = new MongoClient(connectionString);

            // Henter auktions-databasen fra client
            var _mongoDatabase = client.GetDatabase("Auktiondb");

            BudCollection = _mongoDatabase.GetCollection<Bud>("Bud");
        }

        // Henter bud fra _mongoDatabase ("Bud")
        public IMongoCollection<Bud> BudCollection { get; }

    }
}
