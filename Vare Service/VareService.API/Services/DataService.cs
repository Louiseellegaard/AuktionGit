using System.Text.Json;
using MongoDB.Driver;
using VareService.Models;

namespace VareService.Services
{
    public interface IDataService
    {
        Task<List<Vare>> Get();
        Task<Vare> Get(string id);
        Task<Vare> Create(Vare vare);
        Task<Vare> Update(string id, Vare vare);
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

        public async Task<List<Vare>> Get()
        {
            // Find alle varer.
            return await _db
                .VareCollection
                .Find(v => true)
                .ToListAsync();
        }

        public async Task<Vare> Get(string id)
        {
            // Find en enkelt vare.
            return await _db
                .VareCollection
                .Find(v => v.ProductId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Vare> Create(Vare vare)
        {
            // Lav en vare.
            await _db
                .VareCollection
                .InsertOneAsync(vare);

            return vare;
        }

        public async Task<Vare> Update(string id, Vare vare)
        {
            // Opdater en vare.
            await _db
                .VareCollection
                .ReplaceOneAsync(v => v.ProductId == id, vare);

            return vare;
        }

        public async Task Delete(string id)
        {
            // Fjern en vare.
            await _db
                .VareCollection
                .DeleteOneAsync(v => v.ProductId == id);
        }
    }
}