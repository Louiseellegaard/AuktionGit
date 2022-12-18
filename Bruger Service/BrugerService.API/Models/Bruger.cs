using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrugerService.Models;

public class Bruger
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }

    public Bruger()
    {
        UserId = ObjectId.GenerateNewId().ToString();
    }
}
