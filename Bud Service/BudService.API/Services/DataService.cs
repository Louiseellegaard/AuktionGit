using System.Text.Json;
using MongoDB.Driver;
using BudService.Models;

namespace BudService.Services
{
    public interface IDataService
    {
        Task<List<Bud>> Get();
        Task<Bud> Get(string id);
        Task<Bud> Create(Bud bud);
        Task<Bud> Update(string id, Bud bud);
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

        public async Task<List<Bud>> Get()
        {
            // Find alle bud.
            return await _db
                .BudCollection
                .Find(b => true)
                .ToListAsync();
        }

        public async Task<Bud> Get(string id)
        {
            // Find et enkelt bud.
            return await _db
                .BudCollection
                .Find(b => b.BidId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Bud> Create(Bud bud)
        {
            // Lav et bud.
            await _db
                .BudCollection
                .InsertOneAsync(bud);

            return bud;
        }

        public async Task<Bud> Update(string id, Bud Bud)
        {
            // Opdater et bud.
            await _db
                .BudCollection
                .ReplaceOneAsync(b => b.BidId == id, Bud);

            return Bud;
        }

        public async Task Delete(string id)
        {
            // Fjern et bud.
            await _db
                .BudCollection
                .DeleteOneAsync(b => b.BidId == id);
        }
    }
}