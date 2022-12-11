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
    public DateTime? AuctionEnd { get; set; }
    public double MinimumPrice { get; set; }



    // constructor - når nyt auktion-objekt laves, så opretts objektet med et id. 
    public Auktion()
    {
        AuctionId = ObjectId.GenerateNewId().ToString();
    }
}
