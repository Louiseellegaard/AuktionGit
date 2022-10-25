using System.Text.Json;
using VareApi.Controllers;
using VareApi.Models;

namespace Service;

public class DataService
{
    private List<Vare> data { get; }

    //public DataService()
    //{
    //    this.data = new List<Vare>();
    //    this.data.Add(new Vare() { "1", "Bord", "Langt bord", 2, 800, "16:00", "uhewqfjhwegjeg" });
    //    this.data.Add(new Vare() { "2", "Bord", "Kort bord", 3, 850, "17:00", "newgjjg" }));
    //    this.data.Add(new Vare() { "3", "Bord", "Mellem bord", 4, 750, "19:00", "gejwgjhew" )};
    //}
    public DataService() { 
    data.Add(new Vare()
    {
        productId = "1",
        title = "Bord",
        description = "Langt bord",
        showRoomid = 2,
        valuation = 800,
        auktionStart = "16:00",
        images = "gejwfhuwegu"
    });
        data.Add(new Vare()
        {
            productId = "2",
            title = "Bord",
            description = "Mellem bord",
            showRoomid = 3,
            valuation = 850,
            auktionStart = "17:00",
            images = "gejwfrgmkregkhuwegu"
        });
        data.Add(new Vare()
        {
            productId = "3",
            title = "Bord",
            description = "Kort bord",
            showRoomid = 4,
            valuation = 750,
            auktionStart = "18:00",
            images = "geuwegu"
        });
    }

public Vare[] GetVare()
    {
        return data.ToArray();
    }

    public Vare GetVareById(string id)
    {
        return data.Find(vare => vare.productId == id);
    }

    public string CreateVare(string productId, string title, string description, int showRoomid, double valuation, string auktionStart, string images)
    {
        var vare = new Vare()
        {
            productId = productId,
            title = title,
            description = description,
            showRoomid = showRoomid,
            valuation = valuation,
            auktionStart = auktionStart,
            images = images
        };
        data.Add(vare);
        return JsonSerializer.Serialize(new { msg = "Ny vare oprettet", newVare = vare });
    }
}