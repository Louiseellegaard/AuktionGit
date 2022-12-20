﻿using MongoDB.Driver;

using BrugerService.Models;

namespace BrugerService.Services
{
	public interface IDbContext
	{
		IMongoCollection<Bruger> BrugerCollection { get; }
	}

	public class DbContext : IDbContext
	{
		private ILogger<DbContext> _logger;
		private IConfiguration _config;
		public IMongoCollection<Bruger> BrugerCollection { get; }

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
			BrugerCollection = _mongoDatabase.GetCollection<Bruger>(_config["Collection"]);

			_logger.LogInformation("Forbundet til database {database}", _mongoDatabase.DatabaseNamespace.DatabaseName);
			_logger.LogInformation("Benytter collection {collection}", BrugerCollection.CollectionNamespace.CollectionName);
		}
	}
}