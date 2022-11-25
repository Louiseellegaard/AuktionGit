using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KundeApi.Models;

public class Kunde
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string CustomerId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }

    public Kunde()
    {
        CustomerId = ObjectId.GenerateNewId().ToString();
    }
}
