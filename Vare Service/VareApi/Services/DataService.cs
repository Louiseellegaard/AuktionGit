using System.Text.Json;
using VareApi.Controllers;
using VareApi.Models;

namespace VareApi.Services
{
    public interface IDataService
    {
        IEnumerable<Vare> GetAll();
        Vare GetById(string id);
        string Create(Vare vare);
        void Update(Vare vare);
    }

    public class DataService : IDataService
    {
        private static List<Vare> data;

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

        public IEnumerable<Vare> GetAll()
        {
            return data
                .ToList();
        }

        public Vare GetById(string id)
        {
            return data
                .Find(vare => vare.ProductId == id)!;
        }

        public string Create(Vare vare)
        {
            data.Add(vare);
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