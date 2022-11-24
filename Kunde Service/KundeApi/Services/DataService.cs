using System.Text.Json;

using MongoDB.Driver;

using KundeApi.Models;

namespace KundeApi.Services
{
	public interface IDataService
	{
		Task<IEnumerable<Kunde>> GetAll();
		Task Create(Kunde kunde);
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

		public async Task<IEnumerable<Kunde>> GetAll()
		{
			return await _db
				.KundeCollection
				.Find(v => true)
				.ToListAsync();
		}

		public async Task Create(Kunde kunde)
		{
			await _db
				.KundeCollection
				.InsertOneAsync(kunde);
		}
	}
}
