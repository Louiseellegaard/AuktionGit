using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VareService.Models;

public class Vare
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string ProductId { get; set; }
	public ProductCategory Category { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public int ShowRoomId { get; set; }
	public double Valuation { get; set; }
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime AuctionStart { get; set; }
	public string[]? Images { get; set; }

	public Vare()
	{
		ProductId = ObjectId.GenerateNewId().ToString();
	}
}

public enum ProductCategory
{
	None = 0,
	Electronics = 1,
	Cars = 2,
	Hobby = 3,
	Furniture = 4,
	Jewelry = 5,
	SportAndLeisure = 6,
	Textiles = 7,
	Watches = 8,
	Wine = 9
}