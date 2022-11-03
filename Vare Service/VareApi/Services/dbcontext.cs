using MongoDB;
using MongoDB.Driver;
using System;
using VareApi.Models;

namespace VareApi.Services
{
    public interface IDbContext
    {
        IMongoCollection<Vare> VareCollection { get; }
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

            VareCollection = _mongoDatabase.GetCollection<Vare>("Vare");
        }

        // Henter varer fra _mongoDatabase ("Vare")
        public IMongoCollection<Vare> VareCollection { get; }

    }
}