using MongoDB.Driver;
using System.Text.Json;
using VareApi.Models;

namespace VareApi.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Vare>> GetAll();
        Task<Vare> GetById(string id);
        Task Create(Vare vare);
        Task<string> Update(Vare vare);
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

        public async Task<IEnumerable<Vare>> GetAll()
        {
            return await _db
                .VareCollection
                .Find(v => true)
                .ToListAsync();
        }

        public async Task<Vare> GetById(string id)
        {
            return await _db
                .VareCollection
                .Find(v => v.ProductId == id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(Vare vare)
        {
            await _db
                .VareCollection
                .InsertOneAsync(vare);
        }

        public async Task<string> Update(Vare vare)
        {
            await _db
                .VareCollection
                .ReplaceOneAsync(filter: v => v.ProductId == vare.ProductId, replacement: vare);

            return JsonSerializer.Serialize(new { msg = "Vare opdateret", newVare = vare });
        }
    }
}