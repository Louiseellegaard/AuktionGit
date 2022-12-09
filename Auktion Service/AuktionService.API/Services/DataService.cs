using System.Text.Json;
using MongoDB.Driver;
using AuktionService.Models;

namespace AuktionService.Services
{
    public interface IDataService
    {
        Task<List<Auktion>> Get();
        Task<Auktion> Get(string id);
        Task<Auktion> Create(Auktion auktion);
        Task<Auktion> Update(string id, Auktion auktion);
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

        public async Task<List<Auktion>> Get()
        {
            // Find alle auktioner.
            return await _db
                .AuktionCollection
                .Find(a => true)
                .ToListAsync();
        }

        public async Task<Auktion> Get(string id)
        {
            // Find en enkelt auktion.
            return await _db
                .AuktionCollection
                .Find(a => a.AuctionId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Auktion> Create(Auktion auktion)
        {
            // Lav en auktion.
            await _db
                .AuktionCollection
                .InsertOneAsync(auktion);

            return auktion;
        }

        public async Task<Auktion> Update(string id, Auktion Auktion)
        {
            // Opdater en auktion.
            await _db
                .AuktionCollection
                .ReplaceOneAsync(a => a.AuctionId == id, Auktion);

            return Auktion;
        }

        public async Task Delete(string id)
        {
            // Fjern en auktion.
            await _db
                .AuktionCollection
                .DeleteOneAsync(a => a.AuctionId == id);
        }
    }
}