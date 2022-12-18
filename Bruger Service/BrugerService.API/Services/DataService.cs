using System.Text.Json;
using MongoDB.Driver;
using BrugerService.Models;

namespace BrugerService.Services
{
	public interface IDataService
	{
		Task<List<Bruger>> Get();
		Task<Bruger> Get(string id);
		Task<Bruger> Create(Bruger bruger);
		Task<Bruger> Update(string id, Bruger bruger);
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

		public async Task<List<Bruger>> Get()
		{
            // Find alle brugere.
			return await _db
				.BrugerCollection
				.Find(b => true)
				.ToListAsync();
		}

		public async Task<Bruger> Get(string id)
        {
            // Find en enkelt bruger. 
            return await _db
				.BrugerCollection
				.Find(b => b.UserId == id)
				.FirstOrDefaultAsync();
        }

		public async Task<Bruger> Create(Bruger bruger)
		{
			// Lav en bruger.
			await _db
				.BrugerCollection
				.InsertOneAsync(bruger);

			return bruger;
		}

		public async Task<Bruger> Update(string id, Bruger bruger)
        {
            // Opdater en bruger. 
            await _db
			 	.BrugerCollection
			 	.ReplaceOneAsync(b => b.UserId == id, bruger);
            
			return bruger;
        }

        public async Task Delete(string id)
        {
            // Fjern en bruger.
            await _db
				.BrugerCollection
				.DeleteOneAsync(b => b.UserId == id);
        }
	}
}
