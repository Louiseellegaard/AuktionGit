namespace IndexService.Models;

public class AuktionFuld
{
    public string? AuctionId { get; set; }
	public string? AuctionDescription { get; set; }
	public string? ProductId { get; set; }
	public string? ProductTitle { get; set; }
	public string? ProductDescription { get; set; }
	public double ProductValuation { get; set; }
	public double MinimumPrice { get; set; }
	public string? BuyerId { get; set; }
    public ShowRoom ShowRoom { get; set; }
	public DateTime? AuctionStart { get; set; }
	public DateTime? AuctionEnd { get; set; }
    public string[]? Images { get; set; }
	public List<Bud>? Bids { get; set; }
}

public enum ShowRoom
{
	None = 0,
	Viborg = 1,
	Randers = 2,
	Aarhus = 3,
	Viby = 4,
	Tilst = 5,
	Herning = 6,
	Grenå = 7,
	Odense = 8,
	København = 9
}