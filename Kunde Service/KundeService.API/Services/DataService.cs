using System.Text.Json;
using MongoDB.Driver;
using KundeService.Models;

namespace KundeService.Services
{
	public interface IDataService
	{
		Task<List<Kunde>> Get();
		Task<Kunde> Get(string id);
		Task<Kunde> Create(Kunde kunde);
		Task<Kunde> Update(string id, Kunde kunde);
		Task Delete(string id);
	}

	public class DataService : IDataService
	{
		private readonly ILogger<DataService> _logger;
		private readonly IDbContext _db;

		public DataService(ILogger<DataService> logger, IDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		public async Task<List<Kunde>> Get()
		{
            // Find alle kunder. 
			return await _db
				.KundeCollection
				.Find(k => true)
				.ToListAsync();
		}

		public async Task<Kunde> Get(string id)
        {
            // Find en enkelt kunde. 
            return await _db
				.KundeCollection
				.Find(k => k.CustomerId == id)
				.FirstOrDefaultAsync();
        }

		public async Task<Kunde> Create(Kunde kunde)
		{
			// Lav en kunde.
			await _db
				.KundeCollection
				.InsertOneAsync(kunde);

			return kunde;
		}

		public async Task<Kunde> Update(string id, Kunde kunde)
        {
            // Opdater en kunde. 
            await _db
			 	.KundeCollection
			 	.ReplaceOneAsync(k => k.CustomerId == id, kunde);
            
			return kunde;
        }

        public async Task Delete(string id)
        {
            // Fjern en kunde.
            await _db
				.KundeCollection
				.DeleteOneAsync(k => k.CustomerId == id);
        }
	}
}
