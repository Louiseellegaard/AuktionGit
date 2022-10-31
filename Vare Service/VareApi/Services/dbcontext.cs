using MongoDB;
using MongoDB.Driver;
using System;
using VareApi.Models;

namespace VareApi.Services
{
    public class DbContext
    {
        private readonly IMongoDatabase _mongoDatabase;


        public DbContext()
        {
            // Opretter en MongoDB-client med forbindelse til MongoDB Atlas
            var client = new MongoClient("mongodb+srv://louisedb:louisedb123@auktionshusdb.upg5v0d.mongodb.net/?retryWrites=true&w=majority");

            // Henter shelter-databasen fra client
            _mongoDatabase = client.GetDatabase("Auktiondb");

        }


        // Henter shelters fra _mongoDatabase ("Vare")
        public IMongoCollection<Vare> VareCollection
        {
            get
            {
                return _mongoDatabase.GetCollection<Vare>("Vare");
            }
        }

    }
}