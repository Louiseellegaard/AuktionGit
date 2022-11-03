using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VareApi.Models;

public class Vare
    {
    [BsonId]
    public string ProductId { get; set; }   
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ShowRoomId { get; set; }
    public double Valuation { get; set; }
    public string AuctionStart { get; set; }
    public string[]? Images { get; set; }
    }
