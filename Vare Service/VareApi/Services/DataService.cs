using MongoDB.Driver;
using System.Text.Json;
using VareApi.Models;

namespace VareApi.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Vare>> GetAll();
        Task<Vare> GetById(string id);
        Task<string> Create(Vare vare);
        Task<string> Update(Vare vare);
    }

    public class DataService : IDataService
    {
        private readonly ILogger<DataService> _logger;
        private readonly DbContext _db = new DbContext();

        public DataService(ILogger<DataService> logger)
        {
            _logger = logger;
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

        public async Task<string> Create(Vare vare)
        {
            await _db.VareCollection
                .InsertOneAsync(vare);

            return JsonSerializer.Serialize(new { msg = "Ny vare oprettet", newVare = vare });
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