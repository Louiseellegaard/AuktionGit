using System.Text.Json;
using VareApi.Controllers;
using VareApi.Models;
using Auktion.Services.Vareapi;
using MongoDB.Driver;

namespace VareApi.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Vare>> GetAll();
        Vare GetById(string id);
        Task<string>Create(Vare vare);
        void Update(Vare vare);
    }

    public class DataService : IDataService
    {
        private static List<Vare> data;
        private readonly dbcontext _db = new dbcontext();
        public DataService()
        {
            data = new List<Vare>()
            {
                new Vare()
                {
                    ProductId = "1",
                    Title = "Bord",
                    Description = "Langt bord",
                    ShowRoomid = 2,
                    Valuation = 800,
                    AuctionStart = "16:00",
                    Images = new string[] { "image1" }
                },
                new Vare()
                {
                    ProductId = "2",
                    Title = "Bord",
                    Description = "Mellem bord",
                    ShowRoomid = 3,
                    Valuation = 850,
                    AuctionStart = "17:00",
                    Images = new string[] { "image1", "image2" }
                },
                new Vare()
                {
                    ProductId = "3",
                    Title = "Bord",
                    Description = "Kort bord",
                    ShowRoomid = 4,
                    Valuation = 750,
                    AuctionStart = "18:00",
                    Images = new string[] { "image1" }

                }
            };
        }

        public async Task<IEnumerable<Vare>> GetAll()
        {
            return await _db
                .VareCollection.Find(v=>true).ToListAsync();
        }

        public Vare GetById(string id)
        {
            return data
                .Find(vare => vare.ProductId == id)!;
        }

        public async Task<string> Create(Vare vare)
        {
            await _db.VareCollection.InsertOneAsync(vare);

            return JsonSerializer.Serialize(new { msg = "Ny vare oprettet", newVare = vare });
        }
         public void Update (Vare vare)
        {
            int update = data
            .FindIndex(v => v.ProductId == vare.ProductId)!;
            data.RemoveAt(update);
            data.Add(vare);
        }
    }
}