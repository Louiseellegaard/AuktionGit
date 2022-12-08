using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BudService.Models;

public class Bud
{
    [BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string BidId { get; set; }
    public string AuctionId { get; set; }
    public string BuyerId { get; set; }
    public DateTime Date { get; set; }
    public double Bid { get; set; }

    // constructor - når et nyt bud-objekt laves, så har objektet allerede et id. 
    public Bud()
    {
        BidId = ObjectId.GenerateNewId().ToString();
    }
}
