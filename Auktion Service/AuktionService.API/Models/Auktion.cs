using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuktionService.Models;

public class Auktion
{
    [BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? AuctionId { get; set; }
    public string? ProductId { get; set; }
    public string? BuyerId { get; set; }
    public string? Description { get; set; }
    public DateTime EndTime { get; set; }
    public double MinimumPrice { get; set; }

//constructor - når ny bud objekt, så har objektet allerede et id. 
    public Auktion()
    {
        AuctionId = ObjectId.GenerateNewId().ToString();
    }
}
